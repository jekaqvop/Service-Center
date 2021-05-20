using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Service_Center.Converters
{
    class DecimalToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
        string patternDecimal = @"^([0-9\.]{1,50})$";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (Regex.IsMatch(value.ToString(), patternDecimal, RegexOptions.IgnoreCase))
            {
                decimal obj = System.Convert.ToDecimal(value);
                if (obj < 999999 && obj >= 0)
                {
                    return obj;
                }
                else
                    MessageBox.Show("Цена не должна быть меньше 99999 и больше нуля");
            }              
             else
                MessageBox.Show("В цене должны быть только цифры!");
            return 0;
        }
    }
}
