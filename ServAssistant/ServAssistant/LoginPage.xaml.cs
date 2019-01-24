//using Android.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ServAssistant
{
    [Serializable]
    public class User
    {
        [JsonProperty("nazwa")]
        public string nazwa;
        [JsonProperty("haslo")]
        public string haslo;
        [JsonProperty("grupa")]
        public int grupa;
    }

    [Serializable]
    public class Users
    {
        public List<User> uzytkownik { get; set; }

        public Users()
        {
            uzytkownik = new List<User>();
        }
    }

    public partial class LoginPage : ContentPage
    {

        public Users users = new Users();

        public LoginPage()
        {
            InitializeComponent();
            passwordText.Text = "";
            buttonLogin.Clicked += ButtonLogin_Clicked;
            buttonRegister.Clicked += ButtonRegister_Clicked;
        }

        async void RegisterAsync(string path)
        {
            User user = new User
            {
                nazwa = loginText.Text,
                haslo = passwordText.Text,
                grupa = 3
            };

            loginIndicator.IsVisible = true;

            var json = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var getResponse = await client.GetStringAsync(App.api + "uzytkownik?filter=nazwa,eq," + loginText.Text + "&transform=1");
            users = JsonConvert.DeserializeObject<Users>(getResponse);

            if (users.uzytkownik.Count == 1)
            {
                DisplayAlert("Uwaga", "Użytkownik o podanym nicku istnieje!", "OK");
                return;
            }
            else
            {
                buttonRegister.IsEnabled = false;
                buttonLogin.IsEnabled = false;
                buttonRegister.Text = "Rejestracja w toku...";
                var postResposne = await client.PostAsync(path, stringContent);
                buttonRegister.IsEnabled = true;
                buttonLogin.IsEnabled = true;
                buttonRegister.Text = "Zarejestruj się";
                DisplayAlert("Uwaga", "Rejestracja zakończona sukcesem!", "OK");
            }
            loginIndicator.IsVisible = false;
            App.LoggedUser = user;

        }

        async void LoginAsync(string path)
        {
            loginIndicator.IsVisible = true;
            buttonRegister.IsEnabled = false;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var response = await client.GetStringAsync(path);          
                users = JsonConvert.DeserializeObject<Users>(response);
                //jsonLabel.Text = JsonConvert.SerializeObject(users).ToString();
                if (users.uzytkownik.Count == 1)
                {
                    if (users.uzytkownik.First().haslo == passwordText.Text)
                        DisplayAlert("Logowanie", "Zalogowany!", "OK");
                    else
                        DisplayAlert("Uwaga", "Niewłaściwe Hasło!", "OK");
                }
                else
                    DisplayAlert("Uwaga", "Nie ma takiego użytkownika!", "OK");
                buttonRegister.IsEnabled = true;
                buttonLogin.IsEnabled = true;
                buttonLogin.Text = "Zaloguj";
                App.LoggedUser.nazwa = loginText.Text;
            }
            loginIndicator.IsVisible = false;
            await Navigation.PushAsync(new MainTabbedPage());

        }
        private void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            if (CheckCreds())
            {
                RegisterAsync(App.api + "uzytkownik");
            }
        }

        private bool CheckCreds()
        {
            users.uzytkownik.Clear();
            if (passwordText.Text == "")
            {
                DisplayAlert("Uwaga", "Wpisz Hasło!", "OK");
                return false;
            }

            if (loginText.Text == "")
            {
                DisplayAlert("Uwaga", "Wpisz swój login!", "OK");
                return false;
            }
            return true;
        }
        private void ButtonLogin_Clicked(object sender, EventArgs e)
        {         
            if (CheckCreds())
            {
                buttonLogin.IsEnabled = false;
                buttonLogin.Text = "Sprawdzanie...";
                LoginAsync(App.api + "uzytkownik?filter=nazwa,eq," + loginText.Text + "&transform=1");
            }
            

            //throw new NotImplementedException();
        }
    }
}
