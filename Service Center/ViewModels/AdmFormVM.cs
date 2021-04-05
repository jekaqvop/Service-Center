using GalaSoft.MvvmLight.Views;
using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class AdmFormVM : INotifyPropertyChanged
    {
        public AdmFormVM() { }
        AdminWindow adminWindow;
        public UserControl MainUserControl { get; set; }
        public AdmFormVM(AdminWindow adminWindow)
        {
            this.adminWindow = adminWindow;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        
        private int _selectedPage = 1;
        public Grid GridCursor
        {
            get;
            set;
        }
        private Thickness gridCursorMargin = new Thickness(0, 100, 0, 0);
        public Thickness GridCursorMargin { get => gridCursorMargin; set { gridCursorMargin = value; } }
        public int SelectedPage
        {
            get
            {
                return _selectedPage;
            }
            set
            {
                
                _selectedPage = value;

                //adminWindow.TrainsitionigContentSlide.OnApplyTemplate();
                gridCursorMargin = new Thickness(0, (100 + (60 * _selectedPage)), 0, 0);
                OnPropertyChanged("GridCursorMargin");
                OnPropertyChanged("SelectedPage");                
                switch (_selectedPage)
                {
                    case 0:
                        MainUserControl = new TableRapair();
                        OnPropertyChanged("MainUserControl");
                        break;
                    case 1:
                        
                        OnPropertyChanged("MainUserControl");
                        break;
                    case 2:
                        MainUserControl = new UsersControl();
                        OnPropertyChanged("MainUserControl");
                        break;

                }
            }
        }

    }
    
}
