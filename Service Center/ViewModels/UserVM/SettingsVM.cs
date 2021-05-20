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
        Themes Themes = Themes.GetInstance;
        UnitOfWork unitOfWork = new UnitOfWork();
        public SettingsVM()
        {
            ViewManager view = ViewManager.GetInstance;
            user = unitOfWork.Users.GetItem(view.User.UserId);           
        }        
        public bool IsTheme { get => Themes.Theme; set { Themes.Theme = value; OnPropertyChanged("IsTheme"); } }
       
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
                        MessageBox.Show("Такой Login уже есть!\nПридумайте новый.");
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
                OnPropertyChanged("Password");
            }
        }
        bool CheckedLogin(string login)
        {
            ViewManager view = ViewManager.GetInstance;
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(u => u.UserId != view.User.UserId && u.Login.ToUpper() == login.ToUpper());
            if (users.Count() > 0)
                return false;
            return true;
        }
        public string newPassword = "";
        string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{8,16}$";
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword
        {
            get => newPassword;
            set => Set(ref newPassword, value);
        }
        string oldPassword = "";
        public string OldPassword
        {
            get => oldPassword;
            set => Set(ref oldPassword, GetHash(value));
        }
        string patternEmail = @"^[^@\s]{1,25}@[^@\s]{1,10}\.[^@\s]{1,10}$";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email
        {
            get => user.Email;
            set
            {
                if (Regex.IsMatch(value, patternEmail, RegexOptions.None))
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
            ViewManager view = ViewManager.GetInstance;
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(u => u.Email.ToUpper() == email.ToUpper() && u.UserId != view.User.UserId);
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
                    MessageBox.Show("Используйте русский или английский алфавит для ввода ФИО,\nкотопрое должно содержать 2 пробела и каждое слово должно быть с большой буквы.");
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
                {
                    if (CheckedPhone(value))
                        user.PhoneNumber = value;
                    else
                    {
                        MessageBox.Show("Такой номер уже зарегистрирован.");
                    }
                }  
                else
                    MessageBox.Show("Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx");
                OnPropertyChanged("Phone");
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
                if (OldPassword == "" &&  NewPassword == "")
                    unitOfWork.Save();
                else if (!CheckNotNull(typeof(string), OldPassword, NewPassword))
                { 
                    if(Regex.IsMatch(NewPassword, patternPass, RegexOptions.IgnoreCase))
                    {
                        if (user.Password == oldPassword)
                        {
                            user.Password = GetHash(NewPassword);
                            unitOfWork.Save();
                        }
                        else
                        {
                            MessageBox.Show("Указан неверный пароль");
                            newPassword = "";
                            oldPassword = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("В пароле должна быть минимум одна цифра, одна буква, большая буква и любой знак, который не цифра и не буква, максимальная длина пароля 16 символов.");
                        newPassword = "";
                    }
                }
                else
                    MessageBox.Show("Если меняете пароль, то укажите старый и новый пароли");
                OnPropertyChanged("OldPassword");
                OnPropertyChanged("NewPassword");
            });
        }
    }
}
