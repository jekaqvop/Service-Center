using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class RegFormVM : PropertysChanged
    {       
        RegistrationWindow regWin;
        User user;
        public RegFormVM() 
        {
            user = new User();
        }
        public RegFormVM(RegistrationWindow regWin)
        {
            this.regWin = regWin;
            user = new User();
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
                    regWin.Close();
                });
            }
        }
        /// <summary>
        /// Сворачивание окна LoginWindow
        /// </summary>
        public ICommand MinForm
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    regWin.WindowState = WindowState.Minimized;
                });
            }
        }
        string patternPass = @"^[0-9a-zA-Zа-яА-Я]{8,20}$";
        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get { return user.Password; }
            set
            {
                if (Regex.IsMatch(value, patternPass, RegexOptions.IgnoreCase))
                    user.Password = value;
                else
                    MessageBox.Show("Пароль должен содержать минимум 8 символов, 1 верхний, 1 нижний, 1 цифру и максимум 20 / Must contain at least 8 characters, 1 uppercase, 1 lowercase, 1 number and max 20");
                OnPropertyChanged("Password");
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
                    user.Email = value;
                else
                    MessageBox.Show("Проверьте правильнность ввода email / Check the input format email");
                OnPropertyChanged("Email");
            }
        }

        string patternName = @"/^([А-ЯA-Z]|[А-ЯA-Z][\x27а-яa-z]{1,}|[А-ЯA-Z][\x27а-яa-z]{1,}\-([А-ЯA-Z][\x27а-яa-z]{1,}|(оглы)|(кызы)))\040[А-ЯA-Z][\x27а-яa-z]{1,}(\040[А-ЯA-Z][\x27а-яa-z]{1,})?$/";
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName
        {
            get { return user.FullName; }
            set
            {
                if (Regex.IsMatch(value, patternName, RegexOptions.IgnoreCase))
                    user.FullName = value;
                else
                    MessageBox.Show("Используйте русский или английский алфавит для ввода ФИО / Use the Russian or English alphabet to enter a Full name");

                OnPropertyChanged("LastName");
            }
        }

        public string patternPhone = @"^\+[0-9]\d{2}-\d{2}-\d{3}-\d{2}-\d{2}$";
        [Required(ErrorMessage = "Phone is required")]
        public string Phone
        {
            get { return user.PhoneNumber; }
            set
            {
                if (Regex.IsMatch(value, patternPhone, RegexOptions.IgnoreCase))
                    user.PhoneNumber = value;
                else
                    MessageBox.Show("Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx");
                OnPropertyChanged("Phone");
            }
        }
    }
}
