using Newtonsoft.Json;
using ServAssistant.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServAssistant
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActionListPage : ContentPage
	{
        // Obiekt do deserializacji z tabeli Obiekt
        //public Obiekty obiekty { get; set; } = new Obiekty();
        public Akcje akcja = new Akcje();
        public Obiekt currObject;
        public ActionListPage (Obiekt obiekt)
		{

			InitializeComponent ();
            objectName.Text = obiekt.nazwa;
            currObject = obiekt;

            listActions.ItemsSource = akcja.akcja;
            GetActions();

            buttonAdd.Clicked += ButtonAdd_Clicked;
            listActions.ItemTapped += ListActions_ItemTapped;
            checkBoxKonserwacja.CheckedChanged += CheckBoxKonserwacja_CheckedChanged;
            monthsPicker.IsEnabled = false;
            monthsPicker.ItemsSource = new List<string>() { "1 miesiąc", "2 miesiące", "3 miesiące", "4 miesiące", "5 miesięcy", "6 miesięcy", "7 miesięcy", "8 miesięcy", "9 miesięcy", "10 miesięcy", "11 miesięcy", "12 miesięcy"};
		}

        private void CheckBoxKonserwacja_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            monthsPicker.IsEnabled = e.Value;         
        }

        private void ListActions_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new ActionDetails(listActions.SelectedItem as Akcja));
        }

        async void AddAction(Akcja akcja)
        {
            buttonAdd.IsEnabled = false;
            var json = JsonConvert.SerializeObject(akcja);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var postResposne = await client.PostAsync(App.api + "akcja", stringContent);
            buttonAdd.IsEnabled = true;
        }

        async void GetActions()
        {
            Device.BeginInvokeOnMainThread(() => {
                //listActions.BeginRefresh();
                buttonAdd.IsEnabled = false;
            });
            
            var client = new HttpClient();
            akcja.akcja.Clear();
            var getResponse = await client.GetStringAsync(App.api + "akcja?filter=obiekt,eq," + currObject.id.ToString() + "&transform=1");
            var tmp = JsonConvert.DeserializeObject<Akcje>(getResponse);
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (Akcja item in tmp.akcja)
                    akcja.akcja.Add(item);
                //listActions.EndRefresh();
                buttonAdd.IsEnabled = true;
            });
        }

        bool CheckFields()
        {
            if (actionName.Text == "")
            {
                DisplayAlert("Uwaga", "Nie wpisano opisu pracy!", "OK");
                return false;
            }
            return true;
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Akcja akcja = new Akcja();        

            if (CheckFields())
            {
                akcja.nazwa = actionName.Text;
                akcja.obiekt = currObject.id;
                akcja.dataStart = DateTime.Now;
                akcja.zakonczone = false;
                akcja.konserwacja = checkBoxKonserwacja.Checked;
                if (akcja.konserwacja)
                    akcja.okres = monthsPicker.SelectedIndex + 1;
                AddAction(akcja);
                GetActions();
            }
        }
    }
}