﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Base Interface for optical flow algorithms using NVIDIA Optical Flow SDK
    /// </summary>
    public interface INvidiaOpticalFlow : IAlgorithm
    {
        /// <summary>
        /// Pointer the the native cv::cuda::NvidiaOpticalFlow object.
        /// </summary>
        IntPtr NvidiaOpticalFlowPtr { get; }
    }

    public static partial class CudaInvoke
    {
        /// <summary>
        /// Calculates Optical Flow using NVIDIA Optical Flow SDK.
        /// NVIDIA GPUs starting with Turing contain a dedicated hardware accelerator for computing optical flow vectors between pairs of images.
        /// The optical flow hardware accelerator generates block-based optical flow vectors.
        /// The size of the block depends on hardware in use, and can be queried using the function getGridSize().
        /// The block-based flow vectors generated by the hardware can be converted to dense representation(i.e.per-pixel flow vectors) using upSampler() helper function, if needed.
        /// The flow vectors are stored in CV_16SC2 format with x and y components of each flow vector in 16-bit signed fixed point representation S10.5.
        /// </summary>
        /// <param name="nvidiaOpticalFlow">The nvidia optical flow object</param>
        /// <param name="inputImage">Input image</param>
        /// <param name="referenceImage">Reference image of the same size and the same type as input image.</param>
        /// <param name="flow">A buffer consisting of inputImage.Size() / getGridSize() flow vectors in CV_16SC2 format.</param>
        /// <param name="stream">Stream for the asynchronous version.</param>
        /// <param name="hint">Hint buffer if client provides external hints. Must have same size as flow buffer. Caller can provide flow vectors as hints for optical flow calculation.</param>
        /// <param name="cost">Cost buffer contains numbers indicating the confidence associated with each of the generated flow vectors. Higher the cost, lower the confidence. Cost buffer is of type CV_32SC1.</param>
        public static void Calc(
            this INvidiaOpticalFlow nvidiaOpticalFlow,
            IInputArray inputImage,
            IInputArray referenceImage,
            IInputOutputArray flow,
            Stream stream = null,
            IInputArray hint = null,
            IOutputArray cost = null)
        {
            using (InputArray iaInputImage = inputImage.GetInputArray())
            using (InputArray iaReferenceImage = referenceImage.GetInputArray())
            using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
            using (InputArray iaHint = (hint == null ? InputArray.GetEmpty() : hint.GetInputArray()))
            using (OutputArray oaCost = (cost == null ? OutputArray.GetEmpty() : cost.GetOutputArray()))
                cudaNvidiaOpticalFlowCalc(
                    nvidiaOpticalFlow.NvidiaOpticalFlowPtr,
                    iaInputImage,
                    iaReferenceImage,
                    ioaFlow,
                    (stream == null) ? IntPtr.Zero : stream.Ptr,
                    iaHint,
                    oaCost);
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaNvidiaOpticalFlowCalc(
            IntPtr nHWOpticalFlow,
            IntPtr inputImage,
            IntPtr referenceImage,
            IntPtr flow,
            IntPtr stream,
            IntPtr hint,
            IntPtr cost);
    }
}
