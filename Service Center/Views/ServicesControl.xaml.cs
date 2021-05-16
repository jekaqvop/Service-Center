using Service_Center.ViewModels;
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

namespace Service_Center.Views
{
    /// <summary>
    /// Логика взаимодействия для ServicesControl.xaml
    /// </summary>
    public partial class ServicesControl : UserControl
    {
        public ServicesControl()
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