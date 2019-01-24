using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServAssistant.Modele;

namespace ServAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddObjectPage : ContentPage
    {
        public Dictionary<string, int> info = new Dictionary<string, int>();

        public AddObjectPage()
        {
            InitializeComponent();
            buttonAddObject.Clicked += ButtonAddObject_Clicked;

            editCity.Completed += EditCity_Completed;
            editStreet.Completed += EditStreet_Completed;
            infoButtonAdd.Clicked += InfoButtonAdd_Clicked;
        }

        private void InfoButtonAdd_Clicked(object sender, EventArgs e)
        {
            if (infoNazwa.Text == "")
            {
                DisplayAlert("Uwaga!", "Wpisz nazwę dla pola", "OK");
                return;
            }
            else if (infoValue.Text == "")
            {
                DisplayAlert("Uwaga!", "Wpisz wartość dla pola", "OK");
                return;
            }

            info.Add(infoNazwa.Text, int.Parse(infoValue.Text));
            infoNazwa.Text = ""; infoValue.Text = "";
        }

        private void EditStreet_Completed(object sender, EventArgs e)
        {
            editStreet.Text = editStreet.Text.TrimEnd(' ');
            editStreet.Text = App.UppercaseFirst(editStreet.Text);
        }

        private void EditCity_Completed(object sender, EventArgs e)
        {
            editCity.Text = editCity.Text.TrimEnd(' ');
            editCity.Text = App.UppercaseFirst(editCity.Text);
        }

        async void AddObject(Obiekt obiekt)
        {
            var json = JsonConvert.SerializeObject(obiekt);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            var postResposne = await client.PostAsync(App.api + "obiekty", stringContent);
            
        }

        private void ButtonAddObject_Clicked(object sender, EventArgs e)
        {
            if (editCity.Text.Length < 3)
                DisplayAlert("Uwaga", "Wpisz poprawną nazwę miasta", "Ok");
            else if (editStreet.Text.Length < 3)
                DisplayAlert("Uwaga", "Wpisz poprawną nazwę ulicy", "Ok");
            else if (editNumber.Text.Length < 1)
                DisplayAlert("Uwaga", "Wpisz poprawny numer budynku", "Ok");
            else
            {
                Obiekt obiekt = new Obiekt
                {
                    miasto = editCity.Text, ulica=editStreet.Text, numer=int.Parse(editNumber.Text),
                    info = JsonConvert.SerializeObject(info)
                };
                obiekt.nazwa = obiekt.miasto + ", " + obiekt.ulica + " " + obiekt.numer;

                AddObject(obiekt);

                Navigation.PopAsync();
            }
        }
    }
}