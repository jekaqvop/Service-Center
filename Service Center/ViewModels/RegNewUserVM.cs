﻿using Service_Center.Commands;
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
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Input;


namespace Service_Center.ViewModels
{
    class RegNewUserVM : PropertysChanged
    {
        User user;
        public RegNewUserVM()
        {
            user = new User();
        }
        public bool IsButtonEnabled { get; set; }
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
        string GenaratePasswordAndHash()
        {
            Random rnd = new Random();
            int value = rnd.Next(8, 12);
            string newPassword = Membership.GeneratePassword(value, 1);
            SendEmail("Пароль и логин от аккаунта Service Center\n" +
                           $"Ваш логин: {user.Login}\n" +
                           $"Ваш пароль: {newPassword}",
                           user.Email);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            return Convert.ToBase64String(hash);
        }
        void SendEmail(string body, string email)
        {
            MailAddress from = new MailAddress("ServiceCenterLaptop0@gmail.com", "ServiceCenterLaptop0");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to)
            {
                Subject = "Service Center",
                Body = body,
                IsBodyHtml = true
            };
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465)
            {
                Credentials = new NetworkCredential("ServiceCenterLaptop0", "Laptop123"),
                EnableSsl = true
            };
            smtp.Send(m);
        }
        public ICommand Registration
        {
            get => new DelegateCommand(Add, IsValid);
        }
        void Add(object obj)
        {
            user.Password = GenaratePasswordAndHash();
            if (CheckNotNull(typeof(string), user.Login, user.FullName, user.PhoneNumber, user.Email, user.Password))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Users.AddElemet(user);
                    unitOfWork.Save();
                }
                ICommand command = Close;
            }
            else
            {
                MessageBox.Show("Одно из полей не заполнено!");
            }
        }
        public ICommand Close
        {
            get => new DelegateCommand((obj) =>
            {
                ViewManager view = ViewManager.GetInstance;
                view.CloseMiniWindow();
            });
        }
        /// <summary>
        /// Сворачивание окна
        /// </summary>
        public ICommand Minform
        {
            get => new DelegateCommand((obj) =>
            {
                ViewManager view = ViewManager.GetInstance;
                view.MinWindow();
            });
        }
    }
}
