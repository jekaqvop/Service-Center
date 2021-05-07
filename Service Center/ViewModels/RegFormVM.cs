using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using Service_Center.Views;
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
                    user.Login = value;
                    if (!CheckedLogin(value))
                    {
                        MessageBox.Show("Такой Login уже зарегестрирован!");
                        IsButtonEnabled = false;                        
                    }
                    else
                        IsButtonEnabled = true;
                }
                else
                    MessageBox.Show("Логин может содержать только буквы и цифры латинского алфавита / The login can only include letters and numbers of the Latin alphabet");
                OnPropertyChanged("IsButtonEnabled");
                OnPropertyChanged("Password");
            }
        }
        bool CheckedLogin(string login)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList();
            foreach (User user in users)
            {
                if (user.Login == login)
                    return false;
            }
            return true;
        }
        string patternPass = @"^[0-9a-zA-Zа-яА-Я]{8,20}$";
        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get { return user.Password; }
            set
            {
                user.Password = GetHash(value);
                OnPropertyChanged("Password");
            }
        }
        string repeatPassword;
        public string RepeatPassword
        {
            get { return repeatPassword; }
            set
            {               
                repeatPassword = GetHash(value);                
                OnPropertyChanged("RepeatPassword");
            }
        }
        string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email
        {
            get { return user.Email; }
            set
            {
                if (Regex.IsMatch(value, patternEmail, RegexOptions.IgnoreCase))
                {
                    user.Email = value;
                    if (!CheckedEmail(value))
                    {
                        MessageBox.Show("Такой email уже зарегестрирован!");
                        IsButtonEnabled = false;
                    }
                    else
                        IsButtonEnabled = true;
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
            IEnumerable<User> users = unitOfWork.Users.GetItemList();
            foreach (User user in users)
            {
                if (user.Email == email)
                    return false;
            }
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

        public string patternPhone = @"(?:\+375|80)\s?\(?\d\d\)?\s?\d\d(?:\d[\-\s]\d\d[\-\s]\d\d|[\-\s]\d\d[\-\s]\d\d\d|\d{5})";
        [Required(ErrorMessage = "Phone is required")]
        public string Phone
        {
            get => user.PhoneNumber; 
            set
            {
                if (Regex.IsMatch(value, patternPhone, RegexOptions.IgnoreCase))
                    user.PhoneNumber = value;
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
                if (user.Login != null && user.FullName != null && user.PhoneNumber != null && user.Email != null && user.Password != null && RepeatPassword != null)
                {
                    
                    if (Regex.IsMatch(user.Password, patternPass, RegexOptions.IgnoreCase))
                    {
                        if(user.Password == repeatPassword)
                        {
                            using (Context context = new Context())
                            {
                                context.Users.Add(user);
                                context.SaveChanges();
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
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают / Passwords don't match ");
                        }
                    }
                    else
                        MessageBox.Show("Пароль должен содержать минимум 8 символов, 1 верхний, 1 нижний, 1 цифру и максимум 20 / Must contain at least 8 characters, 1 uppercase, 1 lowercase, 1 number and max 20");
                }
                else
                {
                    MessageBox.Show("Одно из полей не заполнено!");
                }   
            });
        }
        /// <summary>
        /// Возвращение окна FАвторизации
        /// </summary>
        public ICommand ShowLoginWindow
        {
            get => new DelegateCommand((obj) =>
            {
                ViewController view = ViewController.GetInstance;
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
                ViewController view = ViewController.GetInstance;
                view.MinWindow();
            });
        }
    }
}
