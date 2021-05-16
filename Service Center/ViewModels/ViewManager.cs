using Newtonsoft.Json;
using Service_Center.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class ThemeAndLang
    {
        bool _LightTheme = true; 
        public bool LightTheme
        {
            get => _LightTheme;
            set
            {
                SetTheme(value);
                _LightTheme = value;
            }
        } 
        public bool _RusEng = false;        
        static ThemeAndLang themeAndLang;
        public static ThemeAndLang GetInstance { get => themeAndLang; }       
        public ThemeAndLang()
        {
           
        }
        ~ThemeAndLang()
        {
            ViewManager view = ViewManager.GetInstance;
            WriteFile(view.User.Login);
        }
        void SetTheme(bool IsTheme)
        {
            ResourceDictionary StyleLight = new ResourceDictionary
            {
                Source = new Uri(@"..\..\Style\LightColors.xaml", UriKind.Relative)
            };
            ResourceDictionary StyleDark = new ResourceDictionary
            {
                Source = new Uri(@"..\..\Style\DarkColors.xaml", UriKind.Relative)
            };
            ResourceDictionary windowStyleDark = new ResourceDictionary
            {
                Source = new Uri(@"..\..\Style\WindowStyle.xaml", UriKind.Relative)
            };
            ResourceDictionary windowStyleLight = new ResourceDictionary
            {
                Source = new Uri(@"..\..\Style\WindowStyleLight.xaml", UriKind.Relative)
            };
            ResourceDictionary styleDarkMaterial = new ResourceDictionary
            {
                Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.RelativeOrAbsolute)
            };
            ResourceDictionary styleLightMaterial = new ResourceDictionary
            {
                Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.RelativeOrAbsolute)
            };
            switch (IsTheme)
            {
                case false:
                    App.Current.Resources.Remove(StyleLight);
                    App.Current.Resources.MergedDictionaries.Add(StyleDark);
                    App.Current.Resources.Remove(windowStyleDark);
                    App.Current.Resources.MergedDictionaries.Add(windowStyleLight);
                    App.Current.Resources.Remove(styleDarkMaterial);
                    App.Current.Resources.MergedDictionaries.Add(styleLightMaterial);
                    break;
                case true:
                    App.Current.Resources.Remove(StyleDark);
                    App.Current.Resources.MergedDictionaries.Add(StyleLight);
                    App.Current.Resources.Remove(windowStyleLight);
                    App.Current.Resources.MergedDictionaries.Add(windowStyleDark);
                    App.Current.Resources.Remove(styleLightMaterial);
                    App.Current.Resources.MergedDictionaries.Add(styleDarkMaterial);
                    break;
            }
        }
        public static ThemeAndLang InitializeComponent(string login)
        {
            return themeAndLang = themeAndLang == null ? ReadFile(login) : themeAndLang;
        }
        static void WriteFile(string userLogin)
        {
            using(StreamWriter writer = new StreamWriter(userLogin + "ThemeAndLang.json"))
            {
                string json = JsonConvert.SerializeObject(themeAndLang, Formatting.Indented);
                writer.Write(json);
            }            
        }
        static ThemeAndLang ReadFile(string userLogin)
        {
            ThemeAndLang themeAndLang = new ThemeAndLang();
            try
            {
                using (StreamReader writer = new StreamReader(userLogin + "ThemeAndLang.json"))
                {
                    string read = writer.ReadToEnd();
                    themeAndLang = JsonConvert.DeserializeObject<ThemeAndLang>(read);
                }
            }
            catch
            {
                WriteFile(userLogin);
            }
            return themeAndLang;
        }
    }
    class ViewManager
    {
        private static ViewManager instance;
        User user;
        public User User 
        { 
            get => user ?? (user = new User()); 
            set 
            {
                user = value;
                ThemeAndLang theme = ThemeAndLang.InitializeComponent(value.Login);
            } 
        }
        public static ViewManager GetInstance { get => instance; }
        Window window { get; set; }
        Window miniWindow { get; set; }
        private static object syncRoot = new Object();
        protected ViewManager(Window window)
        {
            this.window = window;
        }       
        public static ViewManager InitializeComponent(Window window)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    instance = new ViewManager(window);                    
                }
            }
            return instance;
        }
        public void CloseAndShow(Window window)
        {
            this.window.Close();
            this.window = window;
            this.window.Show();
        }
        public void OpenMiniWindow(Window miniWindow)
        {
            this.miniWindow = miniWindow;
            miniWindow.ShowDialog();
        }
        public void CloseMiniWindow()
        {
            this.miniWindow.Close();
        }
        public void MinWindow()
        {
            this.window.WindowState = WindowState.Minimized;
        }       
        
    }
}
