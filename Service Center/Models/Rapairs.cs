using Service_Center.Converters;
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
    class Rapair
    {
       
        [Key]
        public int RapairID { get; set; }
        public int UserID { get; set; }
        string patternDevice = @"^([A-Za-zА-Яа-я1-9\s-]{1,25})$";
        string dervice;
        public string Device
        {
            get => dervice;
            set
            {
                if (Regex.IsMatch(value, patternDevice, RegexOptions.None))
                {
                    dervice = value;
                }
                else
                    MessageBox.Show("В марке устройства могут содержаться только буквы.\nКоличество симовлов от 1 до 25.");               
            }
        }
        string patternSerialNumber = @"^([A-Za-zА-Я1-9]{4,25})$";
        string serialNumber;
        public string SerialNumber
        {
            get => serialNumber;
            set
            {
                if (Regex.IsMatch(value, patternSerialNumber, RegexOptions.IgnoreCase))
                {
                    serialNumber = value;
                }
                else
                    MessageBox.Show("В серийном номре устройства могут содержаться только буквы и цифры.\nКоличество символов от 4 до 25.");
            }
        }
        string patternMalfunction = @"^([A-Za-zА-Яа-я1-9\s-]{1,50})$";
        string malfunction;
        public string Malfunction
        {
            get => malfunction;
            set
            {
                if (Regex.IsMatch(value, patternMalfunction, RegexOptions.IgnoreCase))
                {
                    malfunction = value;
                }
                else
                    MessageBox.Show("В строке неисправность могут содержаться только буквы. Длина строки от 1 до 50 символов.");
            }
        }
        public int CompletedWorks { get; set; }  
        public string Status { get; set; }
        object ShowMessageBox(object value, string message)
        {
            MessageBox.Show(message);
            return value;
        }
        decimal price;
        public decimal SumMoney
        {
            get => price;
            set
            {
                if (value  != -1)                 
                    price = value > 999999 && value < 0 ? (decimal)ShowMessageBox(value, "Цена дожна быть от 0 до 99999") : value;
            }
        }
        public DateTime DateOfRaceipt { get; set; }
    }
}
