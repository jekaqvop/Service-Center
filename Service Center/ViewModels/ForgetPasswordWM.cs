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
using System.Web.Security;
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
        async void SendEmail(string body, string email)
        {
            try
            {
                MailAddress from = new MailAddress("ServiceCenterLaptop0@mail.ru", "ServiceCenterLaptop0");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Service Center";
                m.Body = body;
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.UseDefaultCredentials = false;

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("ServiceCenterLaptop0@mail.ru", "Laptop123");
                await smtp.SendMailAsync(m);
                MessageBox.Show("Новый пароль отправлен вам на email.\nДля безопасности рекомендуется сменить " +
                    "этот пароль на новый!");
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при отправке сообщения!");
            }
        }
        string GetHash(User user)
        {
            Random rnd = new Random();
            int value = rnd.Next(8, 12);
            string newPassword = Membership.GeneratePassword(value, 1);
            SendEmail("Пароль и логин от аккаунта Service Center\n" +                           
                           $"Ваш логин: {user.Login}\n" +
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
                    user.Password = GetHash(user);
                    ViewController view = ViewController.GetInstance;
                    view.CloseMiniWindow();
                }
                else
                {
                    MessageBox.Show("Вы не ввели email\nили ввели его не правильно");
                }
            });
        }
    }
}
