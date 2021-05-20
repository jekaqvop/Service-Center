using Service_Center.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Service_Center.Converters
{
    class IdConvertToTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                UnitOfWork unit = new UnitOfWork();
                if (value != null)
                    return unit.Services.GetItemList().Where(s => s.ServiceId == (int)value).First().Title;
            }
            catch
            {
                return null;
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UnitOfWork unit = new UnitOfWork();
            return unit.Services.GetItemList().Where(s => s.Title == value.ToString()).First().ServiceId;
        }
    }
}
