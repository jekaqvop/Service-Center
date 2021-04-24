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
using System.Drawing;
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
using System.Windows.Media.Imaging;

namespace Service_Center.ViewModels
{    
    class ServicesControlVM : PropertysChanged
    {
       
        public ServicesControlVM()
        {            
            sortServicesList();
        }
        Context context;
        public ObservableCollection<Service> servicesList = new ObservableCollection<Service>();    
        public ObservableCollection<Service> ServicesList { get => servicesList; set { servicesList = value; } }        
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
                using(this.context= new Context())
                {
                    sortServicesList();
                }               
                return price0;
            }
            set => price0 = value;            
        }
        public decimal Price1
        {
            get
            {
                using (this.context= new Context())
                {
                    sortServicesList();
                }
                return price1;
            }
            set => price1 = value;
        }
        
        #endregion
        #region ChangeSelectService
        public int SelectIndex { get; set; }
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
        public byte[] PathImage
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
        
        public string OpenFileDialog()
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
                return openFileDialog.FileName;               
            }
            return null;
        }
       
        public ICommand OpenImage
        {
            get => new DelegateCommand((obj) =>
            {
                string pathImage = OpenFileDialog();
                if (pathImage != null)
                {
                    try
                    {
                        Image image = Image.FromFile(pathImage);
                        MemoryStream ms = new MemoryStream();
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        //PathImage = ms.ToArray();                        
                        OnPropertyChanged("SelectService");
                    }
                    catch
                    {
                        MessageBox.Show("Файл не выбран или произошла ошибка");
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
                sortServicesList();
            });
        }
        #endregion
        #region AddDelUpdateElement
        public ICommand CreateNewAlement
        {
            get => new DelegateCommand((obj) =>
            {  
                context.Services.Add(new Service { Title = "title", Info = "Info", Price = 0});
                sortServicesList();
                OnPropertyChanged("ServicesList");                
            });
        }
        public ICommand DelElement
        {
            get => new DelegateCommand((obj) =>
            {
                context.Services.Local.Remove(selectService);
                sortServicesList();
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
        string getSortParam()
        {
            string getNotNull(string str)
            {
                return str != null ? str : "";
            }
            return getNotNull(sortTitle) + getNotNull(sortPrice) + getNotNull(sortAsc) + getNotNull(sortDesc);
        }
        void sortServicesList()
        {     
            if(context != null)
                context.SaveChanges();
                this.context = new Context();
                switch (getSortParam())
                {
                    case "TASC":
                        context.Services.Where(p => p.Price >= price0 && p.Price <= price1).OrderBy(p => p.Title).Load();
                        ServicesList = context.Services.Local;
                        break;
                    case "PASC":
                        context.Services.Where(p => p.Price >= price0 && p.Price <= price1).OrderBy(p => p.Price).Load();
                        ServicesList = context.Services.Local;
                        break;
                    case "TDESC":
                        context.Services.Where(p => p.Price >= price0 && p.Price <= price1).OrderByDescending(p => p.Title).Load();
                        ServicesList = context.Services.Local;
                        break;
                    case "PDESC":
                        context.Services.Where(p => p.Price >= price0 && p.Price <= price1).OrderByDescending(p => p.Price).Load();
                        ServicesList = context.Services.Local;
                        break;
                    default:
                        context.Services.Load();
                        ServicesList = context.Services.Local;
                        break;
                }
                OnPropertyChanged("ServicesList");                      
        }
        #endregion        
    }

}
