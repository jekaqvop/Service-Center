using Service_Center.Commands;
using Service_Center.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using Service_Center.Contexts;
using System.Data.Entity;
using Service_Center.Resources;
using Service_Center.Repository;
using System.Collections.Generic;

namespace Service_Center.ViewModels
{
    class ServicesControlVM : PropertysChanged
    {
       
        public ServicesControlVM()
        {            
            sortServicesList();            
        }
        UnitOfWork unitOfWork = new UnitOfWork();
        public ObservableCollection<Service> servicesList = new ObservableCollection<Service>();    
        public ObservableCollection<Service> ServicesList 
        { 
            get => servicesList; 
            set
            { 
                servicesList = value;
            } 
        }        
        #region selectingByPrice
        decimal price0 = 0;
        decimal price1 = 9999999;
        public decimal Price0 
        {
            get
            {
                
                    sortServicesList();
                            
                return price0;
            }
            set => price0 = value;            
        }
        public decimal Price1
        {
            get
            {
               
                    sortServicesList();
               
                return price1;
            }
            set => price1 = value;
        }
        
        #endregion
        #region ChangeSelectService
        public int SelectIndex { get; set; }
        Service selectService;
        public Service SelectService
        {
            get
            {
                return selectService ?? unitOfWork.Services.GetFirstItem();
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter =
                "JPEG File(*.jpg)|*.jpg|" +
                "GIF File(*.gif)|*.gif|" +
                "Bitmap File(*.bmp)|*.bmp|" +
                "TIF File(*.tif)|*.tif|" +
                "PNG File(*.png)|*.png|All files (*.*)|*.*"
            };
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
                        PathImage = ms.ToArray();                        
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
                unitOfWork.Save();   
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
                Image image = Image.FromFile(@"D:\Курсач\Service Center\Service Center\Sourse\Images\ImagesServices\DefaultImages.png");
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                byte[] bytes = ms.ToArray();
                unitOfWork.Services.AddElemet(new Service { ImageSourse = bytes ?? null, Title = "title", Info = "Info", Price = 0 });                
                sortServicesList();
                OnPropertyChanged("ServicesList");                
            });
        }
        public ICommand DelElement
        {
             
            get => new DelegateCommand((obj) =>
            {
                if (((Collection<object>)obj).Count > 0)
                {
                    Collection<object> objects = (Collection<object>)obj;
                    List<Service> list = objects.Cast<Service>().ToList();
                    list.ForEach(service =>
                    {
                        unitOfWork.Services.Delete(service.ServiceId);
                        ServicesList.Remove(service);
                    });
                }
                else
                    MessageBox.Show("Не выбраны элементы для удаления");
            });
        }

        #endregion
            #region SortAndSearch

            #region Properties
        string sortAsc;
        string sortDesc;
        string sortTitle;
        string sortPrice;
        string searchTitle = "";
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
        public string SearchTitle
        {
            get
            {
                sortServicesList();
                return searchTitle;
            }
            set => searchTitle = value;
        }
        #endregion
        string getSortParam()
        {
            string getNotNull(string str)
            {
                return str ?? "";
            }
            return getNotNull(sortTitle) + getNotNull(sortPrice) + getNotNull(sortAsc) + getNotNull(sortDesc);
        }        
        void sortServicesList()
        {
            unitOfWork.Save();
                switch (getSortParam())
                {
                    case "TASC":
                    ServicesList  = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)).OrderBy(p => p.Title));                       
                        break;
                    case "PASC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)).OrderBy(p => p.Price));
                        break;
                    case "TDESC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)).OrderByDescending(p => p.Title));
                        break;
                    case "PDESC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)).OrderByDescending(p => p.Price));
                        break;
                    default:
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)));        
                        break;
                }
                OnPropertyChanged("ServicesList");                      
        }
        #endregion        
    }

}
