using Service_Center.Commands;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class RegFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        RegistrationWindow regWin;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }      
        public RegFormVM() { }
        public RegFormVM(RegistrationWindow regWin)
        {
            this.regWin = regWin;
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
                    regWin.Close();
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
                    regWin.WindowState = WindowState.Minimized;
                });
            }
        }

    }
}
