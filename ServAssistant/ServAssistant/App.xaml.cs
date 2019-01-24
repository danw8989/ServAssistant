using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServAssistant
{
    

    public partial class App : Application
    {
        //static public string api = "http://192.168.0.248/crudapi/api.php/";
        static public string api = "http://banity.hekko24.pl/api/api.php/";
        static public User LoggedUser = new User();
        public static Modele.Kategorie kategorie;
        public static Modele.TypyAkcji typ_akcji;

        static public string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public App()
        {
            InitializeComponent();
            GetCategoryInfo();

            MainPage = new NavigationPage(new LoginPage());


        }

        public async void GetCategoryInfo()
        {
            var client = new HttpClient();
            var getResponse = await client.GetStringAsync(App.api + "kategorie?transform=1");
            kategorie = JsonConvert.DeserializeObject<Modele.Kategorie>(getResponse);

            getResponse = await client.GetStringAsync(App.api + "typ_akcji?transform=1");
            typ_akcji = JsonConvert.DeserializeObject<Modele.TypyAkcji>(getResponse);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
