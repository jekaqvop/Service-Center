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

        public UserControl MainUserControl { get; set; }       
        public ICommand ChangeLang
        {
            get => new DelegateCommand((obj) =>
            {

            });
        }        
    }
    
}
