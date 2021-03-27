using Service_Center.Commands;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class LoginFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        LoginWindow logWin;
        public LoginFormVM() { }
        public LoginFormVM(LoginWindow logWin)
        {
            this.logWin = logWin;
        }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public RegistrationWindow Registration
        {
            get;
            set;
        }
        /// <summary>
        /// Открытие формы регистрации
        /// </summary>
        public ICommand OpenRegform
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Registration = new RegistrationWindow();
                    Registration.Show();
                    //logWin.Visibility = Visibility.Hidden;
                });
            }
        }
        /// <summary>
        /// Закрытие формы LoginWindow
        /// </summary>
        public ICommand CloseForm
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    logWin.Close();
                });
            }
        }
        /// <summary>
        /// Сворачивание окна LoginWindow
        /// </summary>
        public ICommand MinForm
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    logWin.WindowState = WindowState.Minimized;
                });
            }
        }
        public ICommand LogInToAccount
        {
            get
            {
                return new DelegateCommand((obj)=>
                {

                });
            }
        }
         
    }
}
