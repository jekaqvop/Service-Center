using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.ViewModels
{
    class RegFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RegistrationWindow _registration_form;
        private MainWindow _main_window;
        
        public RegFormVM(RegistrationWindow regform, MainWindow mainWindow)
        {
            _registration_form = regform;
            _main_window = mainWindow;           
        }
        public RegFormVM() { }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
