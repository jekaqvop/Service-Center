using Service_Center.Repository;
using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Service_Center.Models
{
    class Themes
    {
        [Key]
        public int ThemeID { get; set; }
        public int UserID { get; set; }
        bool _LightTheme = true;
        public bool Theme
        {
            get => _LightTheme;
            set
            {
                SetTheme(value);
                _LightTheme = value;
            }
        }
        static UnitOfWork unitOfWork = new UnitOfWork();
        //public bool _RusEng = false;        
        static Themes themes;
        public static Themes GetInstance { get => themes; }
        public Themes()
        {

        }
        ~Themes()
        {
            //ViewManager view = ViewManager.GetInstance;
            // WriteFile(view.User.Login);

            unitOfWork.Save();
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
        public static Themes InitializeComponent()
        {
            //return Themes = Themes == null ? ReadFile(login) : Themes;            
            if (unitOfWork.Themes.GetItemList().Count() == 0 || unitOfWork.Themes.GetItemList().Where(t => t.UserID == ViewManager.GetInstance.User.UserId).First() == null)
            {
                themes = new Themes { UserID = ViewManager.GetInstance.User.UserId, Theme = true };
                unitOfWork.Themes.AddElemet(themes);
                unitOfWork.Save();
            }
            return themes = unitOfWork.Themes.GetItemList().Where(t => t.UserID == ViewManager.GetInstance.User.UserId).First();
        }
        //static void WriteFile(string userLogin)
        //{
        //    using(StreamWriter writer = new StreamWriter(userLogin + "Themes.json"))
        //    {
        //        string json = JsonConvert.SerializeObject(Themes, Formatting.Indented);
        //        writer.Write(json);
        //    }            
        //}
        //static Themes ReadFile(string userLogin)
        //{
        //    Themes Themes = new Themes();
        //    try
        //    {
        //        using (StreamReader writer = new StreamReader(userLogin + "Themes.json"))
        //        {
        //            string read = writer.ReadToEnd();
        //            Themes = JsonConvert.DeserializeObject<Themes>(read) == null ? new Themes() : JsonConvert.DeserializeObject<Themes>(read);
        //        }
        //    }
        //    catch
        //    {
        //        WriteFile(userLogin);
        //    }
        //    return Themes;
        //}
    }
}
