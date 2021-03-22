using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class LoginFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private MainWindow _registration_form;
        private MainWindow _main_window;
        public LoginFormVM(MainWindow regform, MainWindow mainWindow)
        {
            _registration_form = regform;
            _main_window = mainWindow;
        }
        public LoginFormVM(){ }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
    }
}
