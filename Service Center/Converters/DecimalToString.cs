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
        string patternDecimal = @"^(1-9\.]{1,50})$";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (Regex.IsMatch(value.ToString(), patternDecimal, RegexOptions.None))
                if ((decimal)value > 999999 && (decimal)value < 0)
                {
                    return (decimal)value;
                }
                else
                    MessageBox.Show("Цена не должна быть меньше нуля и больше 99999");
             else
                MessageBox.Show("В цене должны быть только цифры!");
            return -1;
        }
    }
}
