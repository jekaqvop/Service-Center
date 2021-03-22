using Service_Center.Commands;
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
        MainWindow _registration_form;
        MainWindow _main_window;
        RegistrationWindow registration;
        public LoginFormVM() { }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginFormVM(MainWindow regform, MainWindow mainWindow)
        {
            _registration_form = regform;
            _main_window = mainWindow;
        }
        public RegistrationWindow Registration
        {
            get { return registration; }
            set { registration = value; }
        }
        /// <summary>
        /// Открытие формы регистрации
        /// </summary>
        public ICommand OpenRegform
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Registration = new RegistrationWindow();
                    Registration.Show();
                });
            }
        }
         
    }
}
