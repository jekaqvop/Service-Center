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
                        ValidationErrors["Login"] = "Такой Login уже зарегестрирован";
                    else
                    {
                        user.Login = value;
                        ValidationErrors["Login"] = null;
                    }
                }
                else
                {
                    ValidationErrors["Login"] = "Логин может содержать только буквы и цифры латинского алфавита";
                }
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
                if (Regex.IsMatch(value, patternPass, RegexOptions.None))
                    ValidationErrors["Password"] = null;
                else
                    ValidationErrors["Password"] = "В пароле должна быть минимум одна цифра, одна буква, большая буква и любой знак, который не цифра и не буква, максимальная длина пароля 16 символов, минимальная 8.";
                user.Password = value;
                OnPropertyChanged("Password");
            }
        }
        string _repeatPassword = "";
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (user.Password != value)
                {
                    ValidationErrors["RepeatPassword"] = "Пароли не совпадают!";
                }
                else
                    ValidationErrors["RepeatPassword"] = null;
                _repeatPassword = value;
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
                        ValidationErrors["Email"] = "Такой email уже используется!";
                    else
                        ValidationErrors["Email"] = null;
                }
                else
                    ValidationErrors["Email"] = "Проверьте правильнность ввода email / Check the input format email";
                user.Email = value;
                OnPropertyChanged("Email");
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
                {
                    user.FullName = value;
                    ValidationErrors["FullName"] = null;
                }

                else
                    ValidationErrors["FullName"] = "Все первые буквы должны быть заглавными. Строка не должна содержать цифр. В конце не должно быть пробела.";
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
                        ValidationErrors["Phone"] = "Такой номер уже используется!";
                    }
                    else
                    {
                        ValidationErrors["Phone"] = null;
                        user.PhoneNumber = value;
                    }
                }
                else
                    ValidationErrors["Phone"] = "Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx";
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
            get => new DelegateCommand(_addNewUser, IsValid);
        }
        void _addNewUser(object obj)
        {
            ViewManager view = ViewManager.GetInstance;
            if (user.Login != null && user.FullName != null && user.PhoneNumber != null && user.Email != null && user.Password != null)
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
                MessageBox.Show("Заполните все поля!");
            }
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
