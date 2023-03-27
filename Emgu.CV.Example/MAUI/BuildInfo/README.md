This is a basic MAUI project based on the dotnet MAUI template. 

When you click the button on the demo project, it should display the Emgu CV build information.

To make sure you have dotnet MAUI workload installed, open the terminal and run this 
> dotnet workload install maui

To restore the nuget package, in the terminal run this command:
> dotnet restore
 
To build the Mac OS MAUI App and run it, use this command:
> dotnet build -t:Run -f net6.0-maccatalyst

To build the iOS MAUI app and run it on a simulator, use this command:
> dotnet build -t:Run -f net6.0-ios

To build the iOS MAUI .ipa file for iOS device, use this command:
> dotnet publish -f net6.0-ios -c Release -r ios-arm64 --self-contained

To build the iOS MAUI app and run it on an iOS device, use this command:
> dotnet build -t:Run -f net6.0-ios -c Release -r ios-arm64 --self-contained -p:_DeviceName={YOUR_IOS_DEVICE_UUID}

To build the Android MAUI app, first start up an Android simulator (or connect to an Android device), then use the following command and build and run the Android app:
> dotnet build -t:Run -f net6.0-android

For Windows build, you can use Visual Studio on windows.
