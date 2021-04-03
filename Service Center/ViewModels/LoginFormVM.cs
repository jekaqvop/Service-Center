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
        public double OpacityBadPassword
        {
            get;
            set;

        }
        public string Login
        {
            get => login;
            set => login = value;
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
                    if (Registration == null)
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
        

        public ICommand LogInToAccount             
        {
            get{
                    return new DelegateCommand((obj) =>
                    {
                        using (Context context = new Context())
                        {
                            User user = null;

                            IQueryable<User> users = from User in context.Users
                                                     where User.Login == login
                                                     select User;
                            int io = users.Count();
                            if (io == 1)
                            {
                                foreach (User us in users)
                                {
                                    user = us;
                                    break;
                                }
                                string HashPassword = GetHash(logWin.PasswordBox.Password);
                                if (user.Password == HashPassword)
                                {
                                    switch (user.Role)
                                    {
                                        case true:
                                            if (AdminWindow == null)
                                                AdminWindow = new AdminWindow();
                                            AdminWindow.Show();
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
                            else
                            {

                            }
                        }
                    });
            }
            
        }
        
        
         
    }
}
