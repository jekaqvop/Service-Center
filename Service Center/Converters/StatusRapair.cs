using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Service_Center.Converters
{
    enum StatusEnum
    {
        
        WaitingDiagnosis,
        DiagnosticsPerformed,
        ApprovalWithClient,
        RepairsProgress,
        WaitingPayment,
        RepairsPaidFor
    }
    class StatusRapair : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "WaitingDiagnosis":
                    return StatusEnum.WaitingDiagnosis;
                case "DiagnosticsPerformed":
                    return StatusEnum.DiagnosticsPerformed;
                case "ApprovalWithClient":
                    return StatusEnum.ApprovalWithClient;
                case "RepairsProgress":
                    return StatusEnum.RepairsProgress;
                case "WaitingPayment":
                    return StatusEnum.WaitingPayment;
                default:
                    return StatusEnum.WaitingDiagnosis;
            }
        }
     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(value.ToString() == StatusEnum.WaitingPayment.ToString())
            //    SendEmail("Ваше устпройство починено. Пожалуйста, оплатите услуги и заберите своё устройство.", )
            return value.ToString();
        }
    }
}
