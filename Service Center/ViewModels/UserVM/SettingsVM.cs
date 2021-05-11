using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels.UserVM
{
    class SettingsVM : PropertysChanged
    {
        User user;
        public SettingsVM()
        {
            user = new User();
        }
        public bool _IsTheme;
        public bool IsTheme { get => _IsTheme; set => Set(ref _IsTheme, value); }
        public ICommand ChangedTheme
        {
            get => new DelegateCommand((obj) =>
            {
                ResourceDictionary windowStyle = new ResourceDictionary
                {
                    Source = new Uri(@"..\..\Language\langRu2.xaml", UriKind.Relative)
                };
                ResourceDictionary windowStyleLight = new ResourceDictionary
                {
                    Source = new Uri(@"..\..\Language\langEng.xaml", UriKind.Relative)
                };
                switch (IsTheme)
                {
                    case true:
                        App.Current.Resources.Remove(windowStyle);
                        App.Current.Resources.MergedDictionaries.Add(windowStyleLight);

                        break;
                    case false:
                        App.Current.Resources.Remove(windowStyleLight);
                        App.Current.Resources.MergedDictionaries.Add(windowStyle);

                        break;
                }
            });
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
                        MessageBox.Show("Такой Login уже есть!\nПридумайте новый.");
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
            ViewController view = ViewController.GetInstance;
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(u => u.UserId != view.User.UserId && u.Login == login);
            if (users.Count() > 0)
                return false;
            return true;
        }
        string patternPass = @"^[0-9a-zA-Zа-яА-Я]{8,20}$";
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword
        {
            get { return user.Password; }
            set
            {
                user.Password = GetHash(value);
                OnPropertyChanged("Password");
            }
        }
        string oldPassword;
        public string OldPassword
        {
            get => oldPassword;
            set
            {
                user.Password = GetHash(value);
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
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(u => u.Email == email);
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
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                
            });
        }
    }
}
