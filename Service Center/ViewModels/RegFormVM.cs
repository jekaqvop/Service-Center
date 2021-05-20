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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class RegFormVM : PropertysChanged
    {       
        User user;
        public RegFormVM() 
        {
            user = new User();
        }
        public bool IsButtonEnabled { get; set; } = true;
        string patternLog = @"([A-Za-z1-9]{4,25})";
        [Required(ErrorMessage = "Login is required")]
        public string Login
        {
            get => user.Login;
            set
            {
                if (Regex.IsMatch(value, patternLog, RegexOptions.IgnoreCase))
                {
                   
                    if (!CheckedLogin(value))
                    {
                        MessageBox.Show("Такой Login уже зарегестрирован!");
                        IsButtonEnabled = false;                        
                    }
                    else
                    {
                        user.Login = value;
                        IsButtonEnabled = true;
                    }
                }
                else
                    MessageBox.Show("Логин может содержать только буквы и цифры латинского алфавита / The login can only include letters and numbers of the Latin alphabet");
                OnPropertyChanged("IsButtonEnabled");
                OnPropertyChanged("Login");
            }
        }
        bool CheckedLogin(string login)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(u => u.Login.ToUpper() == login.ToUpper());
            if (users.Count() > 0)
                return false;
            return true;
        }
        string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{8,16}$";
        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get => user.Password; 
            set
            {
                user.Password = value;
                OnPropertyChanged("Password");
            }
        }
        string repeatPassword;
        public string RepeatPassword
        {
            get => repeatPassword; 
            set
            {               
                repeatPassword = value;                
                OnPropertyChanged("RepeatPassword");
            }
        }
        string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email
        {
            get => user.Email; 
            set
            {
                if (Regex.IsMatch(value, patternEmail, RegexOptions.IgnoreCase))
                {                    
                    if (!CheckedEmail(value))
                    {
                        MessageBox.Show("Такой email уже зарегестрирован!");
                        IsButtonEnabled = false;
                    }
                    else
                    {
                        user.Email = value;
                        IsButtonEnabled = true;
                    }
                       
                }                  
                else
                    MessageBox.Show("Проверьте правильнность ввода email / Check the input format email");
                OnPropertyChanged("Email");
                OnPropertyChanged("IsButtonEnabled");
            }
        }
        bool CheckedEmail(string email)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(e => e.Email.ToUpper() == email.ToUpper());
            if (users.Count() > 0)
                return false;
            return true;
        }
        string patternName = @"^(([A-ZА-ЯЁ]{1}[a-zа-яё]{1,}[\s]){2}[A-ZА-ЯЁ][a-zа-яё]{1,})$";
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName
        {
            get => user.FullName; 
            set
            {
                if (Regex.IsMatch(value, patternName, RegexOptions.None))
                    user.FullName = value;
                else
                    MessageBox.Show("Используйте русский или английский алфавит для ввода ФИО / Use the Russian or English alphabet to enter a Full name");
                OnPropertyChanged("LastName");
            }
        }
        bool CheckedPhone(string phone)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(e => e.PhoneNumber == phone);
            if (users.Count() > 0)
                return false;
            return true;
        }
        string patternPhone = @"(?:\+375|80)\s?\(?\d\d\)?\s?\d\d(?:\d[\-\s]\d\d[\-\s]\d\d|[\-\s]\d\d[\-\s]\d\d\d|\d{5})";
        [Required(ErrorMessage = "Phone is required")]
        public string Phone
        {
            get => user.PhoneNumber; 
            set
            {
                if (Regex.IsMatch(value, patternPhone, RegexOptions.IgnoreCase))
                {
                    if (!CheckedPhone(value))
                    {
                        MessageBox.Show("Такой номер телефона уже зарегестрирован!");
                        IsButtonEnabled = false;
                    }
                    else
                    {
                        IsButtonEnabled = true;
                        user.PhoneNumber = value;   
                    }    
                }                   
                else
                    MessageBox.Show("Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx");
                OnPropertyChanged("Phone");
            }
        }
        string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
        public ICommand Registration
        {
            get => new DelegateCommand((obj) =>
            {
                ViewManager view = ViewManager.GetInstance;
                if (user.Login != null && user.FullName != null && user.PhoneNumber != null && user.Email != null && user.Password != null && RepeatPassword != null)
                {
                    
                    if (Regex.IsMatch(Password, patternPass, RegexOptions.None))
                    {
                        if(user.Password == repeatPassword)
                        {
                            user.Password = GetHash(user.Password);
                            UnitOfWork unitOfWork = new UnitOfWork();
                            unitOfWork.Users.AddElemet(user);
                            unitOfWork.Save();
                            view.User = this.user;
                                switch (user.Role)
                                {
                                    case true:                                      
                                        view.CloseAndShow(new AdminWindow());
                                        break;
                                    case false:
                                        view.CloseAndShow(new UserWindow());
                                        break;
                                    default:
                                    MessageBox.Show("Не удалось определить профиль пользователя!");
                                        break;
                                }                            
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают / Passwords don't match ");
                        }
                    }
                    else
                        MessageBox.Show("В пароле должна быть минимум одна цифра, одна буква, большая буква и любой знак, который не цифра и не буква, максимальная длина пароля 16 символов, минимальная 8.");
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }   
            });
        }
        /// <summary>
        /// Возвращение окна Авторизации
        /// </summary>
        public ICommand ShowLoginWindow
        {
            get => new DelegateCommand((obj) =>
            {
                ViewManager view = ViewManager.GetInstance;
                view.CloseAndShow(new LoginWindow());
            });
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
    }
}
