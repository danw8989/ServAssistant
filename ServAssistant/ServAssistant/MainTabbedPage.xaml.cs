using Newtonsoft.Json;
using ServAssistant.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        Obiekty obiekty = new Obiekty();

        public MainTabbedPage()
        {
            InitializeComponent();
            buttonAddObject.Clicked += ButtonAddObject_Clicked;
            searchEdit.Completed += SearchEdit_Completed;

            listObjects.ItemsSource = obiekty.obiekty;

            var personDataTemplate = new DataTemplate(() =>
            {

                var nameLabel = new Label { FontAttributes = FontAttributes.Bold, FontSize = 16, VerticalOptions = LayoutOptions.CenterAndExpand};

                nameLabel.SetBinding(Label.TextProperty, "nazwa");

                return new ViewCell { View = nameLabel };
            });

            listObjects.ItemTemplate = personDataTemplate;

            listObjects.ItemTapped += ListObjects_ItemTapped;

            SearchForObjects(false);
        }

        private void ListObjects_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new ObjectTabPage(listObjects.SelectedItem as Obiekt));
        }

        async void SearchForObjects(bool refresh)
        {
            obiekty.obiekty.Clear();
            var client = new HttpClient();

            if (refresh)
                listObjects.BeginRefresh();

            Obiekty tmp = new Obiekty();

            searchEdit.IsEnabled = false;

            var getResponse = await client.GetStringAsync(App.api + "obiekty?filter=nazwa,cs," + searchEdit.Text + "&transform=1");
            tmp = JsonConvert.DeserializeObject<Obiekty>(getResponse);

            foreach (Obiekt item in tmp.obiekty)
            {
                obiekty.obiekty.Add(item);
            }

            if (refresh)
            listObjects.EndRefresh();

            searchEdit.IsEnabled = true;

        }

        private void SearchEdit_Completed(object sender, EventArgs e)
        {
            searchEdit.Text = searchEdit.Text.TrimEnd(' ');
            SearchForObjects(true);
        }

        private void ButtonAddObject_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddObjectPage());
        }
    }
}