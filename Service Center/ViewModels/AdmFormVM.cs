using GalaSoft.MvvmLight.Views;
using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class AdmFormVM : PropertysChanged
    {
        public AdmFormVM() { }
        bool style = true;      
        public ICommand ChangeLang
        {
            get => new DelegateCommand((obj) =>
            {
                ResourceDictionary windowStyle = new ResourceDictionary();
                windowStyle.Source = new Uri(@"Style\WindowStyle.xaml", UriKind.Relative);
                ResourceDictionary windowStyleLight = new ResourceDictionary();
                windowStyleLight.Source = new Uri(@"Style\WindowStyleLight.xaml", UriKind.Relative);
                switch (style)
                {
                    case true:
                        App.Current.Resources.Remove(windowStyle);
                        App.Current.Resources.MergedDictionaries.Add(windowStyleLight);
                        style = false;
                        break;
                    case false:
                        App.Current.Resources.Remove(windowStyleLight);
                        App.Current.Resources.MergedDictionaries.Add(windowStyle);
                        style = true;
                        break;
                }                
               
            });
        }        
    }
    
}
