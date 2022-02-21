using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Views;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.Views;
using MvvmCross;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TakePhotoPage), typeof(TakePhotoPageRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Views
{
    public class TakePhotoPageRenderer : PageRenderer
    {
        #region Services

        private readonly IImageResizeService _imageResizeService = Mvx.IoCProvider.Resolve<IImageResizeService>();

        #endregion

        #region Variables

        private bool _isPreviewVisible = true;
        private AVCaptureSession _captureSession;
        private AVCaptureDeviceInput _captureDeviceInput;
        private AVCaptureStillImageOutput _stillImageOutput;
        private UIView _liveCameraStream;
        private TakePhotoPage _takePhotoPage;
        private Xamarin.Forms.ImageButton _cameraModeButton;
        private Xamarin.Forms.ImageButton _cameraCreateButton;
        private int _photoPageNumber;

        #endregion

        #region Override Methods

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                _takePhotoPage = this.Element as TakePhotoPage;

                _cameraCreateButton = _takePhotoPage.GetPhotoCreateButton();
                _cameraModeButton = _takePhotoPage.GetCameraModeButton();

                _cameraCreateButton.Clicked += TakePhotoButtonTapped;
                _cameraModeButton.Clicked += EditCameraMode;

                _takePhotoPage.ChangeViewPresentation += TempPage_ChangeViewPresentation;

                SetupUserInterface();
                SetupLiveCameraStream();
                AuthorizeCameraUse();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Camera renderer error: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_captureDeviceInput != null && _captureSession != null)
            {
                _captureSession.RemoveInput(_captureDeviceInput);
            }

            if (_captureDeviceInput != null)
            {
                _captureDeviceInput.Dispose();
                _captureDeviceInput = null;
            }

            if (_captureSession != null)
            {
                _captureSession.StopRunning();
                _captureSession.Dispose();
                _captureSession = null;
            }

            if (_stillImageOutput != null)
            {
                _stillImageOutput.Dispose();
                _stillImageOutput = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private void EditCameraMode(object sender, EventArgs e)
        {
            if (_captureDeviceInput == null || _captureDeviceInput.Device == null)
            {
                return;
            }
            AVCaptureDevicePosition devicePosition = _captureDeviceInput.Device.Position;

            devicePosition = devicePosition == AVCaptureDevicePosition.Front
                ? AVCaptureDevicePosition.Back
                : AVCaptureDevicePosition.Front;

            ChangeCameraMode(devicePosition);
        }

        private void ChangeCameraMode(AVCaptureDevicePosition devicePosition)
        {
            AVCaptureDevice device = GetCameraForOrientation(devicePosition);
            ConfigureCameraForDevice(device);

            _captureSession.BeginConfiguration();
            _captureSession.RemoveInput(_captureDeviceInput);
            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.CommitConfiguration();
        }

        private void SetupUserInterface()
        {
            _liveCameraStream = new UIView()
            {
                Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
            };

            View.Add(_liveCameraStream);
            View.SendSubviewToBack(_liveCameraStream);
        }

        private void ChangePhotoVisibility()
        {
            if (!_isPreviewVisible)
            {
                _isPreviewVisible = true;
                _captureSession.StartRunning();
                return;
            }
            _isPreviewVisible = false;
        }

        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            ChangePhotoVisibility();

            AVCaptureConnection cameraConnection = _stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            CoreMedia.CMSampleBuffer sampleBuffer = await _stillImageOutput.CaptureStillImageTaskAsync(cameraConnection);
            NSData jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            var photo = new UIImage(jpegImage);

            CameraType cameraType = _captureDeviceInput.Device.Position == AVCaptureDevicePosition.Front ? CameraType.Front : CameraType.Rear;
            var byteArrayImage = _imageResizeService.ResizeImage(photo, _photoPageNumber, cameraType, 0);
            _takePhotoPage.ViewModel.UserPhoto.Photo = byteArrayImage;
            await _takePhotoPage.ViewModel.CreatePhotoAsync();
        }

        private AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            AVCaptureDevice[] devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (AVCaptureDevice device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }
            return null;
        }

        private void SetupLiveCameraStream()
        {
            _captureSession = new AVCaptureSession();
            CoreAnimation.CALayer viewLayer = _liveCameraStream.Layer;
            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(_captureSession)
            {
                Frame = _liveCameraStream.Bounds
            };
            _liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);

            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            _stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            _captureSession.AddOutput(_stillImageOutput);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.StartRunning();
        }

        private void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        private async void AuthorizeCameraUse()
        {
            AVAuthorizationStatus authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }


        private void TempPage_ChangeViewPresentation(object sender, int arg)
        {
            if (_captureSession != null && !_isPreviewVisible)
            {
                _isPreviewVisible = true;
                _captureSession.StartRunning();
            }
            if (arg == _photoPageNumber)
            {
                return;
            }
            _photoPageNumber = arg;
            if (_captureDeviceInput == null || _captureDeviceInput.Device == null)
            {
                return;
            }
            AVCaptureDevicePosition devicePosition = _captureDeviceInput.Device.Position;
            if (_photoPageNumber > 0 && devicePosition == AVCaptureDevicePosition.Back)
            {
                devicePosition = AVCaptureDevicePosition.Front;
                ChangeCameraMode(devicePosition);
            }
            if (_photoPageNumber == 0)
            {
                devicePosition = AVCaptureDevicePosition.Back;
                ChangeCameraMode(devicePosition);
            }
        }

        #endregion
    }
}