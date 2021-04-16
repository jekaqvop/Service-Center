using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows;
using Service_Center.Contexts;
using System.Data.Entity;
using Service_Center.Converters;
using Service_Center.Resources;

namespace Service_Center.ViewModels
{    
    class ServicesControlVM : PropertysChanged
    {
        Context context;               
        public ServicesControlVM()
        {
            context = new Context();
            context.Services.Load();
            ServicesList = context.Services.Local;
            OnPropertyChanged("ServicesList");
            if (ServicesList.Count == 0)
            {                
                Directory.Delete(ServicesImages, true); //true - если директория не пуста удаляем все ее содержимое
                Directory.CreateDirectory(ServicesImages);
            }
           
        }        
        private ObservableCollection<Service> servicesListView = new ObservableCollection<Service>();
        public ObservableCollection<Service> ServicesList
        {
            get => servicesListView;
            set
            {                
                servicesListView = value;                
                OnPropertyChanged("ServicesList");
            }            
            
        }
        public string UpdateLang { get; set; }
        public ICommand UpdateLangSourse
        {
            get => new DelegateCommand((obj) =>
            {
                //UpdateLang = @" ..\Language\langEng.xaml";
            });
        }

           
        #region selectingByPrice
        decimal price0 = 0;
        decimal price1 = 9999999;
        public decimal Price0 
        {
            get
            {
                selectingItems();
                return price0;
            }
            set => price0 = value;            
        }
        public decimal Price1
        {
            get
            {
                selectingItems();
                return price1;
            }
            set => price1 = value;
        }
        void selectingItems()
        {
            context.SaveChanges();
            ServicesList = new ObservableCollection<Service>(context.Services.Local.Select(p => p).Where(p => p.Price >= price0 && p.Price <= price1));
            sortServicesList();
            OnPropertyChanged("ServicesList");
        }
        #endregion
        #region ChangeSelectService
        Service selectService = new Service();
        public Service SelectService
        {
            get
            {
                return selectService == null ? selectService = new Service() : selectService;
            }
            set
            {
                selectService = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("Info");                
                OnPropertyChanged("Price");
                OnPropertyChanged("PathImage");
            }
        }
        public string Info
        {
            get => SelectService.Info;
            set
            {
                SelectService.Info = value;
                OnPropertyChanged("SelectService");
            }

        }
        public string Title
        {
            get
            {
                return SelectService.Title;
            }
            set
            {
                SelectService.Title = value;
                OnPropertyChanged("SelectService");
            }
        }
        public string PathImage
        {
            get { return SelectService.ImageSourse; }
            set
            {
                SelectService.ImageSourse = value;
                OnPropertyChanged("SelectService");
            }
        }
        public decimal Price
        {
            get => SelectService.Price;
            set
            {
                SelectService.Price = value;
                OnPropertyChanged("SelectService");
            }
        }
       
        string ImagePath;
        string ServicesImages = @"D:\Курсач\Service Center\Service Center\Sourse\Images\ImagesServices\";
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter =
                "JPEG File(*.jpg)|*.jpg|" +
                "GIF File(*.gif)|*.gif|" +
                "Bitmap File(*.bmp)|*.bmp|" +
                "TIF File(*.tif)|*.tif|" +
                "PNG File(*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }         
        public ICommand OpenImage
        {
            get => new DelegateCommand((obj) =>
            {
                OpenFileDialog();
                if (ImagePath != null)
                {
                    try
                    {
                        SelectService.ImageSourse = ServicesImages + selectService.ServiceId.ToString() + Path.GetFileName(ImagePath);
                        File.Copy(ImagePath, SelectService.ImageSourse);
                        OnPropertyChanged("SelectService");
                    }
                    catch
                    {
                        MessageBox.Show("Вы уже добавили это фото\nили возникла оишбка добавления фото");
                    }
                }              
               
            });
        }
        #endregion
        #region SaveUpdate
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                context.SaveChanges();
                MessageBox.Show("Успешно");
            });
        }
        public ICommand Update
        {
            get => new DelegateCommand((obj) =>
            {

                ServicesList = new ObservableCollection<Service>();
                context.Services.Load();
                ServicesList = context.Services.Local;
            });
        }
        #endregion
        #region AddDelUpdateElement
        public ICommand CreateNewAlement
        {
            get => new DelegateCommand((obj) =>
            {
                Service service = new Service { Title = "title", Info = "Info" };
                service.ServiceId = ServicesList.Count + 1;
                ServicesList.Add(service);
                OnPropertyChanged("ServicesList");
                context.SaveChanges();
            });
        }
        public ICommand DelElement
        {
            get => new DelegateCommand((obj) =>
            {
                ServicesList.Remove(SelectService);
                OnPropertyChanged("SelectService");
            });
        }
        #endregion
        #region Sort    
        string sortAsc;
        string sortDesc;
        string sortTitle;
        string sortPrice;
        public string SortAsc 
        {
            get
            {
                sortServicesList();
                return sortAsc;
            }
            set => sortAsc = value;
        } 
        public string SortDesc
        {
            get
            {
                sortServicesList();
                return sortDesc;
            }
            set => sortDesc = value;
        }
        public string SortTitle
        {
            get
            {
                sortServicesList();
                return sortTitle;
            }
            set => sortTitle = value;
        }
        public string SortPrice
        {
            get
            {
                sortServicesList();
                return sortPrice;
            }
            set => sortPrice = value;
        }
        void sortServicesList()
        {
            context.SaveChanges();            
            switch (sortTitle + sortPrice + sortAsc + sortDesc)
            {
                case "TASC":
                    ServicesList = new ObservableCollection<Service>(ServicesList.OrderBy(p=>p.Title));
                    break;
                case "PASC":
                    ServicesList = new ObservableCollection<Service>(ServicesList.OrderBy(p => p.Price));
                    break;
                case "TDESC":
                    ServicesList = new ObservableCollection<Service>(ServicesList.OrderByDescending(p => p.Title));
                    break;
                case "PDESC":
                    ServicesList = new ObservableCollection<Service>(ServicesList.OrderByDescending(p => p.Price));
                    break;                
            }
        }
        #endregion
    }

}
