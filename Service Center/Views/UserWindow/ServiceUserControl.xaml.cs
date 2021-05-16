using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Service_Center.Views.UserWindow
{
    /// <summary>
    /// Логика взаимодействия для ServiceUserControl.xaml
    /// </summary>
    public partial class ServiceUserControl : UserControl
    {
        public ServiceUserControl()
        {
            InitializeComponent();
        }
        private void TitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SeviceList.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SeviceList.Items.Refresh();
        }
      

    }
}
