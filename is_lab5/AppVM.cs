using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace is_lab5
{
    public class AppVM : INotifyPropertyChanged
    {
        #region Variables
        public CefSharp.Wpf.ChromiumWebBrowser? WebBrowser { get; set; }
        public static string? access_token;
        public static string? user_id;
        public static string? methodURI;
        public static string? URIstr;
        private Command? authCommand;
        private Command? showProfileInfoCommand;
        private Command? showGroupsInfoCommand;
        private Command? showCityUniCommand;

        private readonly string AppID;
        private readonly string ClientSecret;

        static readonly HttpClient httpClient = new();
        
        public Visibility authBV = Visibility.Visible;
        public Visibility AuthBV
        {
            get { return authBV; }
            set { 
                authBV = value;
                OnPropertyChanged("AuthBV");
            }
        }

        public Visibility methodsBV = Visibility.Hidden;
        public Visibility MethodsBV
        {
            get { return methodsBV; }
            set
            {
                methodsBV = value;
                OnPropertyChanged("MethodsBV");
            }
        }

        #endregion

        public AppVM()
        {
            AppID = "clien id приложения!";
            ClientSecret = "секретный ключ приложения!";
        }



        #region Commands

        public Command AuthCommand
        {
            get => authCommand ??= new Command(obj =>
            {
                if (WebBrowser is null)
                    return;
                WebBrowser.AddressChanged += WebBrowser_AddressChanged;

                var uriStr = @"https://oauth.vk.com/authorize?client_id=" + AppID + @"&scope=friends,groups,phone_number&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=5.154&response_type=code";
                WebBrowser.Load(uriStr);
                AuthBV = Visibility.Hidden;
            });
        }

        public Command ShowProfileInfoCommand
        {
            get => showProfileInfoCommand ??= new Command(obj =>
            {
                if (methodURI is null || user_id is null)
                    return;
                GetProfileInfo();
            });
        }

        public Command ShowGroupsInfoCommand
        {
            get => showGroupsInfoCommand ??= new Command(obj =>
            {
                if (methodURI is null || user_id is null)
                    return;
                GetGroupsInfo();
            });
        }

        public Command ShowCityUniCommand
        {
            get => showCityUniCommand ??= new Command(obj =>
            {
                if (methodURI is null || user_id is null)
                    return;
                GetUniversities();
            });
        }

        #endregion



        private void WebBrowser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var uri = new Uri((string)e.NewValue);
            if (uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html#code"))
            {
                string? code = HttpUtility.ParseQueryString(uri.Fragment.Trim('#')).Get("code");
                if (code is null)
                {
                    MessageBox.Show(uri.AbsoluteUri, "Bad uri");
                }
                URIstr = @"https://oauth.vk.com/access_token?client_id=" + AppID + "&client_secret=" + ClientSecret + "&redirect_uri=https://oauth.vk.com/blank.html&code=" + code;
                
                //try-catch?
                GetMehodParams(URIstr); //Вызовет ошибку при неудачной авторизации
                MethodsBV = Visibility.Visible;
            }
        }



        #region post-browser methods

        private static async void GetMehodParams(string accessResponseUri)
        {
            var response = await GET(accessResponseUri);
            if (response.TryGetProperty("access_token", out var dbg))
            {
                access_token = response.GetProperty("access_token").ToString();
                user_id = response.GetProperty("user_id").ToString();

                methodURI = @"https://api.vk.com/method/{0}?{1}&access_token=" + access_token + "&v=5.154";
                //Версия API раз в какое-то время прекращает поддерживаться, актуальная версия тут: https://vk.com/dev/constant_version_updates
            }
        }

        private static async void GetProfileInfo()
        {
            var response = await GET(string.Format(methodURI, "account.getProfileInfo", "fields=first_name,last_name,sex,bdate,city"));
            MessageBox.Show(response.GetProperty("response").ToString());
            try
            {
                //В идеале, нужно создать отдельный класс, который содержит нужные параметры и к которому можно обращаться
                JsonElement responseElement = response.GetProperty("response");
                string firstName = responseElement.GetProperty("first_name").GetRawText();
                string secondName = responseElement.GetProperty("last_name").GetRawText();
                string sex = responseElement.GetProperty("sex").GetRawText();
                string bdate = responseElement.GetProperty("bdate").GetRawText();
                string city = responseElement.GetProperty("city").GetRawText();

                MessageBox.Show($"Имя: {firstName}\nФамилия: {secondName}\nПол: {sex}\nДата рождения: {bdate}\nГород: {city}", "Информация о пользователе");
            }
            catch (Exception e)
            { MessageBox.Show("Возникла ошибка:" + e.Message, "Ошибка"); }
        }

        private static async void GetGroupsInfo()
        {
            var response = await GET(string.Format(methodURI, "groups.get", "&count=4"));
            MessageBox.Show(response.GetProperty("response").ToString());
            try
            {
                JsonElement responseElement = response.GetProperty("response");
                string count = responseElement.GetProperty("count").GetRawText();
                string groups = responseElement.GetProperty("items").GetRawText();

                MessageBox.Show($"Количество групп: {count}\nПервые 4 группы: {groups}", "Информация о группах");
            }
            catch (Exception e)
            { MessageBox.Show("Возникла ошибка:" + e.Message, "Ошибка"); }

        }

        private static async void GetUniversities()
        {
            var response = await GET(string.Format(methodURI, "account.getProfileInfo", "fields=country,city"));

            try
            {
                JsonElement responseElement = response.GetProperty("response");
                string? city_id = responseElement.GetProperty("city").GetProperty("id").GetRawText();
                if (city_id is null || city_id == "")
                    city_id = "1";

                response = await GET(string.Format(methodURI, "database.getUniversities", $"city_id={city_id}&count=10"));
                MessageBox.Show(response.GetProperty("response").ToString());

                responseElement = response.GetProperty("response");
                string count = responseElement.GetProperty("count").GetRawText();
                string unis = responseElement.GetProperty("items").GetRawText();

                /*JsonElement unis = responseElement.GetProperty("items");
                List<string> uniNames = new List<string>();

                for (int i = 0; i < unis.GetArrayLength(); i++)
                {
                    JsonElement universityElement = unis[i];
                    string universityName = universityElement.GetProperty("name").GetRawText();
                    uniNames.Add(universityName);
                }*/

                MessageBox.Show($"Количество: {count}\nУниверситеты(10): {unis}"/*\nНазвания: {uniNames}"*/, "Университеты города");
            }
            catch (Exception e) 
            { MessageBox.Show("Возникла ошибка:" + e.Message, "Ошибка"); }
        }

        #endregion



        private static async Task<JsonElement> GET(string url)
        {
            var json = await httpClient.GetStringAsync(url);
            return JsonDocument.Parse(json).RootElement;
        }

        /*public static T ToObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
