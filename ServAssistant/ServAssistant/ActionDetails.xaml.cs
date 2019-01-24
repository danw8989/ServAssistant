using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServAssistant.Modele;
using Newtonsoft.Json;
using System.Net.Http;
using ServAssistant.Modele;

namespace ServAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionDetails : ContentPage
    {
        public Akcja currAction;
        public Urzadzenia urzadzenie = new Urzadzenia();
        public int deviceCount = 1;
        // Dodawanie urządzenia wraz z listą urządzeń
        public ActionDetails(Akcja akcja)
        {
            InitializeComponent();
            currAction = akcja;
            actionTypePicker.ItemsSource = App.typ_akcji.typ_akcji.Select(s => s.nazwa).ToList();
            categoryPicker.ItemsSource = App.kategorie.kategorie.Select(s => s.nazwa).ToList();
            listDevices.ItemsSource = urzadzenie.urzadzenie;

            addDeviceButton.Clicked += AddDeviceButton_Clicked;
            numberOfDevices.Completed += NumberOfDevices_Completed;
            listDevices.ItemTapped += ListDevices_ItemTapped;
            acceptActionButton.Clicked += AcceptActionButton_Clicked;

            if (akcja.konserwacja)
            {
                actionTypePicker.SelectedIndex = 6;
                actionTypePicker.IsVisible = false;
                actionTypePicker.IsEnabled = false;
                acceptActionButton.IsVisible = false;
                conservationLabel.Text += akcja.dataStart.AddMonths(akcja.okres).ToShortDateString();
            }
            else
            {
                conservationLabel.IsVisible = false;
                if (akcja.zakonczone)
                {
                    acceptActionButton.IsEnabled = false;
                    acceptActionButton.Text = "Zakończona!";
                }
            }

            GetDevices();
        }

        private async void AcceptActionButton_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Pytanie", "Czy napewno chcesz zakończyć tą pracę?", "Tak", "Nie");

            if (answer)
            {
                currAction.zakonczone = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    acceptActionButton.IsEnabled = false;
                    acceptActionButton.Text = "Praca w toku...";
                });

                var json = JsonConvert.SerializeObject(new { zakonczone = true });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                var postResposne = await client.PutAsync(App.api + "akcja/" + currAction.id, stringContent);

                Device.BeginInvokeOnMainThread(() =>
                {
                    acceptActionButton.Text = "Zakończone!";
                });
            }

        }

        private void ListDevices_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new DevicePage(e.Item as Urzadzenie));
        }

        private void NumberOfDevices_Completed(object sender, EventArgs e)
        {
            if (numberOfDevices.Text != "")
                deviceCount = int.Parse(numberOfDevices.Text);
        }

        async void GetDevices()
        {
            Device.BeginInvokeOnMainThread(() => { listDevices.BeginRefresh(); addDeviceButton.IsEnabled = false; });
            var client = new HttpClient();
            urzadzenie.urzadzenie.Clear();
            var getResponse = await client.GetStringAsync(App.api + "urzadzenie?filter=akcja,eq," + currAction.id.ToString() + "&transform=1");
            var tmp = JsonConvert.DeserializeObject<Urzadzenia>(getResponse);

            Device.BeginInvokeOnMainThread(() =>
            {
               foreach (Urzadzenie item in tmp.urzadzenie)
                   urzadzenie.urzadzenie.Add(item);
                listDevices.EndRefresh(); addDeviceButton.IsEnabled = true;
            });

        }

        async void AddDeviceAsync(Urzadzenie device)
        {

            var json = JsonConvert.SerializeObject(device);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            var postResposne = await client.PostAsync(App.api + "urzadzenie", stringContent);
        }

        bool CheckFields()
        {
            if (deviceName.Text == "")
            {
                DisplayAlert("Uwaga", "Nie wpisano nazwy urządzenia!", "OK");
                return false;
            }
            else if (categoryPicker.SelectedItem == null)
            {
                DisplayAlert("Uwaga", "Nie wpisano nazwy urządzenia!", "OK");
                return false;
            }
            else if (actionTypePicker.SelectedItem == null)
            {
                DisplayAlert("Uwaga", "Nie wpisano typu wykonywanej pracy!", "OK");
                return false;
            }
            return true;
        }

        private static string RandomString(int length)
        {
            Random random = new Random();

            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private void AddDeviceButton_Clicked(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                Urzadzenie urzadzenie = new Urzadzenie();
                urzadzenie.nazwa = deviceName.Text;
                urzadzenie.typ_akcji = App.typ_akcji.typ_akcji.Find(s => s.nazwa == actionTypePicker.SelectedItem.ToString()).id;
                urzadzenie.kategoria = App.kategorie.kategorie.Find(s => s.nazwa == categoryPicker.SelectedItem.ToString()).id;
                urzadzenie.akcja = currAction.id;
                for (int i = 0; i < deviceCount; i++)
                {
                    urzadzenie.imgUri = RandomString(16) + "_";
                    AddDeviceAsync(urzadzenie);
                }
                GetDevices();
            }

        }
    }
}