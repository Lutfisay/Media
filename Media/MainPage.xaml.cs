using Plugin.Media;
using System;
using Xamarin.Forms;

namespace Media
{
    public partial class MainPage : ContentPage
    {
        Image _img;
        public MainPage()
        {
            InitializeComponent();

            RelativeLayout layout = new RelativeLayout();
            CustomButton btnTakePhoto = new CustomButton
            {
                Text = "Take Photo"
            };
            btnTakePhoto.Clicked += BtnTakePhoto_Clicked;
            CustomButton btnPickPhoto = new CustomButton
            {
                Text = "Pick Photo"
            };
            btnPickPhoto.Clicked += BtnPickPhoto_Clicked;
            CustomButton btnTakeVideo = new CustomButton
            {
                Text = "Take Video"
            };
            btnTakeVideo.Clicked += BtnTakeVideo_Clicked;
            CustomButton btnPickVideo = new CustomButton
            {
                Text = "Pick Vİdeo"
            };
            btnPickVideo.Clicked += BtnPickVideo_Clicked;
            StackLayout stkImage = new StackLayout
            {
                BackgroundColor = Color.White
            };
            _img = new Image
            {
                Source = "defaultimg.png"
            };
            stkImage.Children.Add(_img);

            layout.Children.Add(stkImage, Constraint.Constant(0),
                Constraint.Constant(0), Constraint.RelativeToParent(
                    (parent) =>
                    {
                        return parent.Width;
                    }));

            StackLayout stkPictureButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 20,
                Children =
                {
                    btnTakePhoto,
                    btnPickPhoto,
                }
            };
            StackLayout stkVideoButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 20,
                Children =
                {
                    btnTakeVideo,
                    btnPickVideo,
                }
            };

            layout.Children.Add(stkPictureButtons, Constraint.Constant(0),
                Constraint.Constant(0), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }));

            layout.Children.Add(stkVideoButtons, Constraint.Constant(0),
                Constraint.RelativeToView(stkPictureButtons,
                (parent, sibling) =>
                {
                    return sibling.Height + 10;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }));

            Content = layout;

        }

        private async void BtnPickVideo_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await DisplayAlert("UYARI", "Galeriye erişme yetkiniz yok!", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickVideoAsync();

            if (file == null)
                return;

            await DisplayAlert("UYARI", "Seçilen video: " + file.Path, "OK");
            file.Dispose();
        }

        private async void BtnTakeVideo_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("UYARI", "Cihazınızın kamerası aktif değil!", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(
                new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = DateTime.Now + ".mp4",
                    Directory = "MediaPluginPhotoVideo",
                    Quality = Plugin.Media.Abstractions.VideoQuality.High,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
                });

            if (file == null)
                return;

            await DisplayAlert("UYARI","Video başarılı bir şekilde kayıt edildi: " + file.Path, "OK");

            file.Dispose();
        }

        private async void BtnPickPhoto_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
               DisplayAlert("UYARI", "Galeriye erişme yetkiniz yok!", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            
            if (file == null)
                return;

            _img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void BtnTakePhoto_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
               await  DisplayAlert("UYARI", "Cihazınızın kamerası aktif değil!", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "MediaPluginPhoto",
                    Name = DateTime.Now + ".jpg",
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
                });

            if (file == null)
                return;

            _img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}
