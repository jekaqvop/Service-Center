using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using Service_Center.Views;
using Service_Center.Views.UserWindow;
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
                    ViewManager view = ViewManager.GetInstance;
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
                ViewManager view = ViewManager.GetInstance;
                view.MinWindow();
            });
        }
        public ICommand ShowForgotPassword
        {
            get => new DelegateCommand((obj) =>
            {
                ViewManager view = ViewManager.GetInstance;
                view.OpenMiniWindow(new ForgetPasswordWind());
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
                    //try
                    //{
                        if (!CheckNotNull(typeof(string), Login, Password))
                        {
                            OpasityProgressBar = 1;

                            UnitOfWork unit = new UnitOfWork();

                            User user = null;

                            IEnumerable<User> users = from User in unit.Users.GetItemList()
                                                      where User.Login.ToUpper() == login.ToUpper()
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
                                    ViewManager view = ViewManager.GetInstance;
                                    view.User = user;
                                    switch (user.Role)
                                    {
                                        case true:
                                            view.CloseAndShow(new AdminWindow());
                                            break;
                                        case false:
                                            view.CloseAndShow(new UserWindow());
                                            break;
                                        default:
                                            MessageBox.Show("Ошибка авторизации!\nПользователь не опознан!\nНеизвесно: администратор или пользователь!");
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
                        else
                            MessageBox.Show("Заполните все поля!");
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("Ошибка подключения или неполадки сервера");
                    //}                       
                });
            }
        }
    }
}
