using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
        string login;
        bool role;
        string hashPassword;
        public RegistrationWindow Registration
        {
            get;
            set;
        }
        public AdminWindow AdminWindow
        {
            get;
            set;
        }
        bool loginTrue = false;
        public string Login 
        { 
            get => login; 
            set
            {
                using (Context context = new Context())
                {
                    IQueryable<User> users = from User in context.Users
                                             where User.Login == value
                                             select User;                    
                    int io = users.Count();
                    login = value;
                    if (io == 1)
                    {
                        foreach (User us in users)
                        {
                            loginTrue = true;
                            hashPassword = us.Password;
                            role = us.Role;
                            break;
                        }
                        logWin.LoginBox.BorderBrush = System.Windows.Media.Brushes.White;
                    }
                    else
                    {
                        loginTrue = false;
                        logWin.LoginBox.BorderBrush = System.Windows.Media.Brushes.Red;
                        
                    }
                }
            } 
        }
        public LoginFormVM() { }
        public LoginFormVM(LoginWindow logWin)
        {
            this.logWin = logWin;
            
           
        }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    if(Registration == null)
                        Registration = new RegistrationWindow();
                    Registration.ShowDialog();
                    Registration = null;                  
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
                    Application.Current.Shutdown();
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
        /// <summary>
        /// Хеширование входной строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        public ICommand LogInToAccount => new DelegateCommand((obj) =>
        {    
            if (loginTrue)
                using (Context context = new Context())
                {
                    string HashPassword = GetHash(logWin.PasswordBox.Password);
                    if (hashPassword == HashPassword)
                    {
                        switch (role)
                        {
                            case true:
                                if (AdminWindow == null)
                                    AdminWindow = new AdminWindow();
                                logWin.Close();
                                logWin = null;
                                break;
                            case false:

                                break;
                            default:

                                break;
                        }
                    }
                }
            else;
        });
        
         
    }
}
