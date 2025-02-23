//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Example.iOS
{
    public class ButtonMessageImageDialogViewController : DialogViewController
    {
        StyledStringElement _button;
        UIImageView _imageView;
        StringElement _messageElement;
        ProgressView _progressView;

        public ButtonMessageImageDialogViewController()
             : base(new RootElement(""), true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            RootElement root = Root;
            root.UnevenRows = true;

            _imageView = new UIImageView(View.Frame);
            _messageElement = new StringElement("");

            _button = new StyledStringElement("", delegate
            {
                if (OnButtonClick != null)
                {
                    if (_progressView == null)
                        _progressView = new ProgressView();

                    _progressView.Show("Please wait",
                    delegate ()
                 {
                      OnButtonClick(this, new EventArgs());
                  });
                }
            }
            );
            root.Add(new Section() { _button });
            root.Add(new Section() { _messageElement });
            root.Add(new Section() { _imageView });
        }

        public Size FrameSize
        {
            get
            {
                int width = 0, height = 0;
                InvokeOnMainThread(delegate
                {
                    width = (int)View.Frame.Width;
                    height = (int)View.Frame.Height;
                });
                return new Size(width, height);
            }
        }

        public string ButtonText
        {
            get { return _button.Caption; }
            set
            {
                _button.Caption = value;
                Root.Reload(_button, UITableViewRowAnimation.None);
            }
        }

        public string MessageText
        {
            get { return _messageElement.Value; }
            set
            {
                InvokeOnMainThread(delegate
                {

                    _messageElement.Value = value;
                    Root.Reload(
                     _messageElement,
                     UITableViewRowAnimation.None
                );
                });
            }
        }

        public void SetImage(IInputArray image)
        {
            InvokeOnMainThread(delegate
            {
                using (InputArray iaImage = image.GetInputArray())
                using (Mat mat = iaImage.GetMat())
                using (UIImage i = mat.ToUIImage())
                {
                    _imageView.Frame = new CGRect(
                    CGPoint.Empty,
                    i.Size
                 );
                    _imageView.Image = i;
                    _imageView.SetNeedsDisplay();
                    ReloadData();
                }
            });
        }

        public event EventHandler<EventArgs> OnButtonClick;

        private static String ByteToSizeStr(long byteCount)
        {
            if (byteCount < 1024)
            {
                return String.Format("{0} B", byteCount);
            }
            else if (byteCount < 1024 * 1024)
            {
                return String.Format("{0} KB", byteCount / 1024);
            }
            else
            {
                return String.Format("{0} MB", byteCount / (1024 * 1024));
            }
        }

        protected void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            String msg;
            if (totalBytesToReceive > 0)
                msg = String.Format("{0} of {1} downloaded ({2}%)", ByteToSizeStr(bytesReceived), ByteToSizeStr(totalBytesToReceive.Value), progressPercentage);
            else
                msg = String.Format("{0} downloaded", ByteToSizeStr(bytesReceived));
            SetMessage(msg);
        }

        public void SetMessage(String msg)
        {
            MessageText = msg;
        }

    }

    public class ProgressView : UIAlertView
    {
        private UIActivityIndicatorView _activityView;

        public ProgressView()
            : base()
        {
        }

        public void Show(string title, Action action)
        {
            Title = title;
            Show();

            if (_activityView == null)
            {
                _activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);

                _activityView.Frame = new CGRect(
                   ((int)Bounds.Width / 2) - 15,
                   (int)Bounds.Height - 50,
                    30,
                    30
                );
            }

            AddSubview(_activityView);

            _activityView.StartAnimating();

            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    action();
                }
                finally
                {

                    this.InvokeOnMainThread(delegate
                 {
                      _activityView.StopAnimating();
                      _activityView.RemoveFromSuperview();
                      DismissWithClickedButtonIndex(0, false);
                  });
                }
            });
        }

    }
}

