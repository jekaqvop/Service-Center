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
using System.ComponentModel;
using System;
using System.Text.RegularExpressions;

namespace Service_Center.ViewModels
{
    class ServicesControlVM : PropertysChanged
    {
        public ServicesControlVM()
        {         
        }
        UnitOfWork unitOfWork = new UnitOfWork();
        public ObservableCollection<Service> servicesList = new ObservableCollection<Service>();    
        public ObservableCollection<Service> ServicesList 
        { 
            get => servicesList;
            set => Set<ObservableCollection<Service>>(ref servicesList, value);            
        }     
        object ShowMessageBox(object value, string message)
        {
            MessageBox.Show(message);
            return value;
        }
        #region selectingByPrice
        decimal price0 = 0;
        decimal price1 = 9999999;
        string patternPrice = @"[0-9]";
        public string Price0 
        {
            get
            {                
                sortServicesList();                            
                return price0.ToString();
            }
            set
            {
                if (Regex.IsMatch(value, patternPrice, RegexOptions.IgnoreCase))
                {
                    decimal dec = decimal.Parse(value);
                    price0 = dec > 999999 && dec < 0 ? (decimal)ShowMessageBox(value, "Цена дожна быть от 0 до 99999") : dec;
                }
                else
                    MessageBox.Show("Цена должна содержать только цифры!");
            }
        }
        public string Price1
        {
            get
            {
                sortServicesList();
                return price1.ToString();
            }
            set
            {
                if (Regex.IsMatch(value, patternPrice, RegexOptions.IgnoreCase))
                {
                    decimal dec = decimal.Parse(value);
                    price1 = dec > 999999 && dec < 0 ? (decimal)ShowMessageBox(value, "Цена дожна быть от 0 до 99999") : dec;
                }
                else
                    MessageBox.Show("Цена должна содержать только цифры!");
            }
        }

        #endregion
        #region ChangeSelectService
        public int SelectIndex { get; set; }
        Service selectService;
        public Service SelectService
        {
            get => selectService ?? unitOfWork.Services.GetFirstItem();            
            set
            {
                Set<Service>(ref selectService, value);
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
                if (value.Length < 500)
                {
                    SelectService.Info = value;
                }
                else
                    MessageBox.Show("Количество симовлов должно быть от 0 до 500.");
                OnPropertyChanged("SelectService");
            }
        }
        string patternTitle = @"^([A-Za-zА-Яа-я1-9\s]{1,25})$";       
        public string Title
        {
            get => SelectService.Title;
            set
            {
                if (Regex.IsMatch(value, patternTitle, RegexOptions.None))
                {
                    SelectService.Title = value;
                }
                else
                    MessageBox.Show("В марке устройства могут содержаться только буквы.\nКоличество симовлов от 1 до 25.");
                OnPropertyChanged("SelectService");
            }
        }
     
        public byte[] PathImage
        {
            get => SelectService.ImageSourse; 
            set
            {
                SelectService.ImageSourse = value;
                OnPropertyChanged("SelectService");
            }
        }
        public string Price
        {
            get => SelectService.Price.ToString();
            set
            {
                if (Regex.IsMatch(value, patternPrice, RegexOptions.IgnoreCase))
                {
                    decimal dec = decimal.Parse(value);
                    SelectService.Price = dec > 999999 && dec < 0 ? (decimal)ShowMessageBox(value, "Цена дожна быть от 0 до 99999") : dec;
                }
                else
                    MessageBox.Show("Цена должна содержать только цифры!");
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
                Random random = new Random();
                Service service = new Service { ImageSourse = bytes ?? null, Title = $"title{random.Next()}", Info = "Info", Price = 0 };
                unitOfWork.Services.AddElemet(service);                
                sortServicesList();
                SelectService = service;
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
            set
            {
                if (value != null && value.Length < 25)
                {
                    searchTitle = value; 
                }
                else
                    MessageBox.Show("Поисковая строка не должна быть пустой и не превышать длину 35 символов!");               

            }
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
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle) && p.Title != "Нет").OrderBy(p => p.Title));                       
                        break;                                                                                                                                                                       
                    case "PASC":                                                                                                                                                                     
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle) && p.Title != "Нет").OrderBy(p => p.Price));
                        break;                                                                                                                                                                       
                    case "TDESC":                                                                                                                                                                    
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle) && p.Title != "Нет").OrderByDescending(p => p.Title));
                        break;                                                                                                                                                                       
                    case "PDESC":                                                                                                                                                                    
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle) && p.Title != "Нет").OrderByDescending(p => p.Price));
                        break;                                                                                                                                                                       
                    default:                                                                                                                                                                         
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle) && p.Title != "Нет"));        
                        break;
                }         
        }    
        #endregion
    }

}
