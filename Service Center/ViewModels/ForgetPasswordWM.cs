using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class ForgetPasswordWM : PropertysChanged
    {
        string email;
        string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                if (Regex.IsMatch(value, patternEmail, RegexOptions.IgnoreCase))
                    email = value;
                else
                    MessageBox.Show("Проверьте правильнность ввода email / Check the input format email");
                OnPropertyChanged("Email");
            }
        }
        void SendEmail(string body, string email)
        {
            MailAddress from = new MailAddress("eavhcev@gmail.com", "Service Center");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Service Center";
            m.Body = body;
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("eavhcev@gmail.com", "oop2020SEM");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
        string GetHash()
        {
            string newPassword = "12345678";
            SendEmail("Пароль и логин от аккаунта Service Center\n" +                           
                           $"Ваш пароль: {newPassword}",
                           email);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            return Convert.ToBase64String(hash);
        }
        public ICommand GenerationNewPassword
        {
            get => new DelegateCommand((obj) =>
            {
                if(email != null && email != "")
                {
                    UnitOfWork unitOfWork = new UnitOfWork();
                    User user = unitOfWork.Users.GetItemList().Where(us => us.Email == email).First();
                    user.Password = GetHash();
                }
                else
                {
                    MessageBox.Show("Вы не ввели email\nили ввели его не правильно");
                }
            });
        }
    }
}
