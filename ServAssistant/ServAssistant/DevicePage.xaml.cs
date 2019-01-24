using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServAssistant.Modele;
using System.ComponentModel;
using Android.Content;
using Android.App;
using XLabs.Forms.Controls;

namespace ServAssistant
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DevicePage : ContentPage
	{
        public Urzadzenie currDevice;
        public List<Image> images = new List<Image>();
        public List<ImageButton> imageButtons = new List<ImageButton>();
        public int imageCount = 1;
        public static Intent intent = new Intent();
        
        public DevicePage (Urzadzenie device)
		{
			InitializeComponent ();
            currDevice = device;
            imageButton.Clicked += ImageButton_ClickedAsync;

            ShowImages();
            string[] files = Directory.GetFiles("/storage/emulated/0/Android/data/danw.servassistant/files/",
                currDevice.imgUri + "*", SearchOption.AllDirectories);
            if (files.Count() == 0)
            {
                /*  Task.Factory.StartNew(() =>
                {
                    DownloadImages();
                }).ContinueWith(task =>
                 {
                    buttonSend.Text = "Ściągnieto";
                });*/
                
                DownloadImages();
                buttonSend.Text = "Ściągnięto";
                ShowImages();
            }

        }


        public void DownloadImages()
        {          
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("banity", "TBplGQw8k");
            for (int i = 1; i < 7; i++)
                try
                {
                    client.DownloadFile(new Uri(
                        "ftp://s21.hekko.pl/images/" + currDevice.imgUri + i.ToString() + ".jpg"),
                        "/storage/emulated/0/Android/data/danw.servassistant/files/Pictures/"
                        + currDevice.imgUri + i.ToString() + ".jpg");
                }
                catch (Exception e)
                {
                    DisplayAlert("Error", e.Message, "OK");
                }
          
        }

        public void ShowImages()
        {
            string[] files = Directory.GetFiles("/storage/emulated/0/Android/data/danw.servassistant/files/",
                currDevice.imgUri + "*", SearchOption.AllDirectories);
            
            imageCount = 1;



            foreach (string item in files)
            {
                FileInfo f = new FileInfo(item);
                if (f.Length > 0)
                {
                    switch (imageCount)
                    {
                        case 1: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        });
                            imageGrid.Children.Add(imageButtons[0], 0 , 0); break;
                        case 2: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        }); imageGrid.Children.Add(imageButtons[1], 1, 0); break;
                        case 3: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        }); imageGrid.Children.Add(imageButtons[2], 2, 0); break;
                        case 4: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        }); imageGrid.Children.Add(imageButtons[3], 0, 1); break;
                        case 5: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        }); imageGrid.Children.Add(imageButtons[4], 1, 1); break;
                        case 6: images.Add(new Image { Source = item }); imageButtons.Add(new ImageButton {
                            Source = item,
                            HeightRequest = 200,
                            ImageWidthRequest = (int)(Width),
                            ImageHeightRequest = (int)(Height)
                        }); imageGrid.Children.Add(imageButtons[5], 2, 1); break;
                    }
                    
                    imageCount++;
                }
                else File.Delete(item);
            }

        }
        
        public async void SendImage(string uri)
        {

        }

        private async void ImageButton_ClickedAsync(object sender, EventArgs e)
        {
            Image PhotoImage = new Image();

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Name = (currDevice.imgUri + imageCount.ToString()),
                PhotoSize = PhotoSize.Small
                
            });

            if (file == null)
                return;



            await DisplayAlert("File Location", file.Path, "OK");


            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            


            Device.BeginInvokeOnMainThread(() => {
                buttonSend.Text = "Wysyłanie" + Path.GetFileName(file.Path);
                switch (imageCount)
                {
                    case 1: imageGrid.Children.Add(PhotoImage, 0, 0); break;
                    case 2: imageGrid.Children.Add(PhotoImage, 1, 0); break;
                    case 3: imageGrid.Children.Add(PhotoImage, 2, 0); break;
                    case 4: imageGrid.Children.Add(PhotoImage, 0, 1); break;
                    case 5: imageGrid.Children.Add(PhotoImage, 1, 1); break;
                    case 6: imageGrid.Children.Add(PhotoImage, 2, 1); break;
                }
                imageCount++;
            });

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("banity", "TBplGQw8k");
                client.UploadFileAsync(new Uri("ftp://s21.hekko.pl/images/" + Path.GetFileName(file.Path)) , WebRequestMethods.Ftp.UploadFile, file.Path);
            }
            buttonSend.Text = "Wysłano";
        }
    }
}