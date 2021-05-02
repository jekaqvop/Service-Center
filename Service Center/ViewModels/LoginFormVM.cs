using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class LoginFormVM : PropertysChanged
    {
        static LoginFormVM()
        {
            using (Context context = new Context()) { }
        }
        string login;       
        public double OpacityBadPassword
        {
            get;
            set;
        }
        public double OpasityProgressBar { get; set; }
        public string Login
        {
            get => login;
            set => login = value;
        }
        public LoginFormVM() { }      
        
        /// <summary>
        /// Открытие формы регистрации
        /// </summary>
        public ICommand OpenRegform
        {
            get
            {
                return new DelegateCommand((obj) =>
                {                   
                    ViewController view = ViewController.GetInstance;
                    view.CloseAndShow(new RegistrationWindow());
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
        /// Сворачивание окна
        /// </summary>
        public ICommand MinForm
        {
            get => new DelegateCommand((obj) =>
            {
                ViewController view = ViewController.GetInstance;
                view.MinWindow();
            });
        }
        public string Password { get; set; }
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
        
        /// <summary>
        /// Проверка логина и пароля
        /// </summary>
        public ICommand LogInToAccount             
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    try
                    {
                        OpasityProgressBar = 1;
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
                                string HashPassword = GetHash(Password);
                                if (user.Password == HashPassword)
                                {
                                    switch (user.Role)
                                    {
                                        case true:                                                
                                            ViewController view = ViewController.GetInstance;
                                            view.CloseAndShow(new AdminWindow());                                                                                         
                                            break;
                                        case false:

                                            break;
                                        default:

                                            break;
                                    }
                                }
                                else
                                {
                                    OpasityProgressBar = 0;
                                    OpacityBadPassword = 1;
                                    OnPropertyChanged("OpasityProgressBar");
                                    OnPropertyChanged("OpacityBadPassword");
                                }
                            }
                            else
                            {
                                OpasityProgressBar = 0;
                                OpacityBadPassword = 1;
                                OnPropertyChanged("OpasityProgressBar");
                                OnPropertyChanged("OpacityBadPassword");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка подключения или неполадки сервера");
                    }                       
                });
            }
        }
    }
}
