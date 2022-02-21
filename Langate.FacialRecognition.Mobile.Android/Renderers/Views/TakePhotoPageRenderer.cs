using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware;
using Android.Views;
using Android.Widget;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Views;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.Views;
using MvvmCross;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TakePhotoPage), typeof(TakePhotoPageRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Views
{
    public class TakePhotoPageRenderer : PageRenderer, TextureView.ISurfaceTextureListener
    {
        #region Services

        private readonly IImageResizeService _imageResizeService = Mvx.IoCProvider.Resolve<IImageResizeService>();

        #endregion

        #region Variables

        private Context _context;
        private string _focusMode;
        private bool _isPreviewVisible = true;
        //private bool _flashOn;

        private global::Android.Hardware.Camera _camera;
        private global::Android.Views.View _view;
        private Activity _activity;
        private CameraFacing _cameraType;
        private TextureView _textureView;
        private SurfaceTexture _surfaceTexture;
        private TakePhotoPage _takePhotoPage;
        private Xamarin.Forms.ImageButton _cameraModeButton;
        private Xamarin.Forms.ImageButton _cameraCreateButton;
        private int _photoPageNumber;
        private int _previewHeightChange;

        #endregion

        public TakePhotoPageRenderer(Context context) : base(context)
        {
            _context = context;
            //_focusMode = Android.Hardware.Camera.Parameters.FocusModeAuto;
            _focusMode = Android.Hardware.Camera.Parameters.FocusModeContinuousPicture;
        }

        #region Override Methods
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            _previewHeightChange = 0;

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                _takePhotoPage = this.Element as TakePhotoPage;

                if (_context.CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    TrySendErrorMessage();
                    return;
                }

                _takePhotoPage.ChangeViewPresentation += TakePhotoPage_ChangeViewPresentation;

                SetupUserInterface();
                AddView(_view);


                _cameraModeButton = _takePhotoPage.GetCameraModeButton();
                _cameraCreateButton = _takePhotoPage.GetPhotoCreateButton();

                _cameraCreateButton.Clicked += TakePhotoButtonTapped;
                _cameraModeButton.Clicked += EditCameraMode;

                TakePhotoPage_ChangeViewPresentation(null, _takePhotoPage.GetCurrentPhotoNumber());
            }
            catch (System.Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine(@"Camera renderer error: ", ex.Message);
                TrySendErrorMessage();
                //ChangePhotoVisibility();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            int msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            int msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            if (_view != null)
            {
                _view.Measure(msw, msh);
                _view.Layout(0, 0, r - l, b - t);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            TryReleaseCamera();
        }

        #endregion

        #region Listeners

        public async void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            try
            {
                _camera = global::Android.Hardware.Camera.Open((int)_cameraType);
            }
            catch (System.Exception ex)
            {
                //_camera = global::Android.Hardware.Camera.Open();
                TryGetCameraWithoutMode();

                //PrepareAndStartCamera();
                //_cameraType = CameraFacing.Back;
                //_camera.Release();
                //_camera = global::Android.Hardware.Camera.Open((int)_cameraType);
            }

            //var sizeCamera = new Android.Hardware.Camera.Size(_camera, width, height);

            Android.Hardware.Camera.Parameters parameters = _camera.GetParameters();
            if (_cameraType != CameraFacing.Front)
            {
                parameters.FocusMode = _focusMode;
                _camera.SetParameters(parameters);
            }

            Android.Hardware.Camera.Size previewSize = parameters.PreviewSize;

            //var _previewWidth = Math.Max(Resources.DisplayMetrics.WidthPixels, previewSize.Height);
            var _previewWidth = Resources.DisplayMetrics.WidthPixels;
            var _previewHeight = (Resources.DisplayMetrics.WidthPixels * previewSize.Width) / previewSize.Height;

            _previewHeightChange = Resources.DisplayMetrics.HeightPixels - _previewHeight;
            if (_previewHeightChange < 0)
            {
                _previewHeightChange = 0;
            }

            _textureView.LayoutParameters = new FrameLayout.LayoutParams(_previewWidth, _previewHeight, GravityFlags.Center);
            _surfaceTexture = surface;

            if (_camera != null)
            {
                _camera.SetPreviewTexture(surface);
                PrepareAndStartCamera();
            }

            //if (_photoPageNumber >= 0 && _camera != null)
            //{
            //    ChangeCameraMode();
            //}
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (_camera != null)
            {
                //_camera.StopPreview();
                _camera.Release();
            }
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            PrepareAndStartCamera();
            if (_camera != null)
            {
                ChangeCameraMode();
            }
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

        #endregion

        #region Private Methods

        private void EditCameraMode(object sender, EventArgs e)
        {
            _cameraType = _cameraType == CameraFacing.Front
                ? CameraFacing.Back
                : CameraFacing.Front;

            TryReleaseCamera();
            ChangeCameraMode();
        }

        private void TryReleaseCamera()
        {
            if (_camera != null)
            {
                _camera.Release();
            }
        }

        private void ChangeCameraMode()
        {
            _camera = global::Android.Hardware.Camera.Open((int)_cameraType);

            Android.Hardware.Camera.Parameters parameters = _camera.GetParameters();

            if (_cameraType != CameraFacing.Front)
            {
                parameters.FocusMode = _focusMode;
                _camera.SetParameters(parameters);
            }

            _camera.SetPreviewTexture(_surfaceTexture);
            PrepareAndStartCamera();
        }

        private void SetupUserInterface()
        {
            _activity = this.Context as Activity;
            _view = _activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
            _cameraType = CameraFacing.Back;

            _textureView = _view.FindViewById<TextureView>(Resource.Id.textureView);
            _textureView.SurfaceTextureListener = this;
        }

        private void PrepareAndStartCamera()
        {
            if (_camera == null)
            {
                return;
            }
            _camera.StopPreview();

            var info = new global::Android.Hardware.Camera.CameraInfo();
            global::Android.Hardware.Camera.GetCameraInfo((int)_cameraType, info);
            int orientation = info.Orientation;
            int changeFrontCamera = 0;
            if (_cameraType == CameraFacing.Front && orientation >= 180)
            {
                changeFrontCamera = -180;
            }
            if (_cameraType == CameraFacing.Front && orientation <= 180)
            {
                changeFrontCamera = 180;
            }
            _camera.SetDisplayOrientation(orientation + changeFrontCamera);
            _camera.StartPreview();
        }

        private void ChangePhotoVisibility()
        {
            if (!_isPreviewVisible)
            {
                _isPreviewVisible = true;
                _camera.StartPreview();
                return;
            }
            _isPreviewVisible = false;
            //_camera.SetPreviewCallback(null);
            _camera.StopPreview();
        }

        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            try
            {
                //ChangePhotoVisibility();
                _isPreviewVisible = false;
                _camera.StopPreview();

                await Task.Run(async () =>
                {
                    var byteArrayImage = _imageResizeService.ResizeImage(_textureView.Bitmap, _photoPageNumber,
                        Models.Enums.CameraType.Default, _previewHeightChange);

                    _takePhotoPage.ViewModel.UserPhoto.Photo = byteArrayImage;
                    await _takePhotoPage.ViewModel.CreatePhotoAsync();
                });

                //await SavePhotoToDevice(croppedPhoto);
            }
            catch (System.Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine(@"Camera renderer: Take photo error ", ex.Message);
                TrySendErrorMessage();
                //ChangePhotoVisibility();
            }
            finally
            {
                TryReleaseCamera();
            }
        }

        private void TakePhotoPage_ChangeViewPresentation(object sender, int arg)
        {
            try
            {
                if (_camera != null && !_isPreviewVisible)
                {
                    _isPreviewVisible = true;
                    _camera.StartPreview();
                }
                if (sender != null && (int)sender == 1)
                {
                    TryReleaseCamera();
                    return;
                }
                if (arg == _photoPageNumber)
                {
                    return;
                }
                if (arg < 0 && _camera != null)
                {
                    ChangeCameraMode();
                    return;
                }
                if (arg > 0 && _cameraType == CameraFacing.Back)
                {
                    _photoPageNumber = arg;
                    _cameraType = CameraFacing.Front;
                    TryReleaseCamera();
                    ChangeCameraMode();
                }
                if (arg == 0)
                {
                    _photoPageNumber = arg;
                    _cameraType = CameraFacing.Back;
                    TryReleaseCamera();
                    ChangeCameraMode();
                }
            }
            catch (System.Exception)
            {
                ChangeCameraMode();
            }
        }

        private async Task SavePhotoToDevice(Bitmap image)
        {
            try
            {
                string absolutePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
                string folderPath = absolutePath + "/Camera";
                string filePath = System.IO.Path.Combine(folderPath, string.Format("photo_{0}.jpg", Guid.NewGuid()));

                var fileStream = new FileStream(filePath, FileMode.Create);
                await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 20, fileStream);
                fileStream.Close();
                image.Recycle();

                var intent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
                var file = new Java.IO.File(filePath);
                var uri = Android.Net.Uri.FromFile(file);
                intent.SetData(uri);
                RootActivity.Instance.SendBroadcast(intent);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"Camera Renderer: SavePhotoToDevice error ", ex.Message);
                TrySendErrorMessage();
                ChangePhotoVisibility();
            }
        }

        private void TrySendErrorMessage()
        {
            if (_takePhotoPage != null)
            {
                _takePhotoPage.ViewModel.ShowErrorMessageAsync(Constants.CameraErrorText);
            }
        }

        private void TryGetCameraWithoutMode()
        {
            try
            {
                _camera = global::Android.Hardware.Camera.Open();
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                TrySendErrorMessage();
            }
        }

        #endregion
    }
}