using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        WaitingPayment
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
            return value.ToString();
        }
    }
}
