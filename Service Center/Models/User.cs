using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Service_Center.Models
{
    class User
    {
        [Key]
        public int UserId { get; set; }
        string login;
        string patternLog = @"([A-Za-z1-9]{4,25})";
        [Required(ErrorMessage = "Login is required")]
        public string Login
        {
            get => login;
            set
            {
                if (Regex.IsMatch(value, patternLog, RegexOptions.IgnoreCase))                
                        login = value;                     
                else
                    MessageBox.Show("Логин может содержать только буквы и цифры латинского алфавита / The login can only include letters and numbers of the Latin alphabet");
            }
        }
        string fullName;
        string patternName = @"^(([A-ZА-ЯЁ]{1}[a-zа-яё]{1,}[\s]){2}[A-ZА-ЯЁ][a-zа-яё]{1,})$";
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName
        {
            get => fullName;
            set
            {
                if (Regex.IsMatch(value, patternName, RegexOptions.None))
                    fullName = value;
                else
                    MessageBox.Show("Используйте русский или английский алфавит для ввода ФИО / Use the Russian or English alphabet to enter a Full name");
            }
        }
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
            }
        }
        string phoneNumber;
        string patternPhone = @"(?:\+375|80)\s?\(?\d\d\)?\s?\d\d(?:\d[\-\s]\d\d[\-\s]\d\d|[\-\s]\d\d[\-\s]\d\d\d|\d{5})";
        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (Regex.IsMatch(value, patternPhone, RegexOptions.IgnoreCase))                
                    phoneNumber = value;                
                else
                    MessageBox.Show("Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx");
            }
        }
        public string Password { get; set; }
        public bool Role { get; set; }
    }
}
