using Service_Center.Commands;
using Service_Center.Converters;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Input;

namespace Service_Center.ViewModels.UserVM
{
    class UserServiceVM : PropertysChanged
    {
        public UserServiceVM()
        {
            sortServicesList();
        }
        UnitOfWork unitOfWork = new UnitOfWork();
        public ObservableCollection<Service> servicesList = new ObservableCollection<Service>();
        public ObservableCollection<Service> ServicesList
        {
            get => servicesList;
            set => Set(ref servicesList, value);
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
            set => Set(ref price1, value);
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
                Set<Service>(ref selectService, value);
                OnPropertyChanged("SelectTitle");
                OnPropertyChanged("SelectInfo");
                OnPropertyChanged("SelectPrice");
                OnPropertyChanged("SelectImage");
            }
        }
        public string SelectInfo
        {
            get => SelectService.Info;
            set
            {
                SelectService.Info = value;
                OnPropertyChanged("SelectService");
            }

        }
        public string SelectTitle
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
        public byte[] SelectImage
        {
            get { return SelectService.ImageSourse; }
            set
            {
                SelectService.ImageSourse = value;
                OnPropertyChanged("SelectService");
            }
        }
        public decimal SelectPrice
        {
            get => SelectService.Price;
            set
            {
                SelectService.Price = value;
                OnPropertyChanged("SelectService");
            }
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
                    MessageBox.Show("Поисковая строка не должна быть пустой и не превышать длину 25 символов!");
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
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && (p.Title + p.Info + p.Price).Contains(searchTitle) && p.Title != "Нет").OrderBy(p => p.Title));
                    break;
                case "PASC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && (p.Title + p.Info + p.Price).Contains(searchTitle) && p.Title != "Нет").OrderBy(p => p.Price));
                    break;
                case "TDESC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && (p.Title + p.Info + p.Price).Contains(searchTitle) && p.Title != "Нет").OrderByDescending(p => p.Title));
                    break;
                case "PDESC":
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && (p.Title + p.Info + p.Price).Contains(searchTitle) && p.Title != "Нет").OrderByDescending(p => p.Price));
                    break;
                default:
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && (p.Title + p.Info + p.Price).Contains(searchTitle) && p.Title != "Нет"));
                    break;
            }           
        }

        #endregion
        public string Device { get; set; }
        public string SerialNumber { get; set; }
        public byte[] PathImage { get; set; }

        //void addRapair(User user)
        //{
        //    Rapair rapair = new Rapair
        //    {
        //        Status = this.StatusNewRapair,
        //        Device = this.Device,
        //        Malfunction = this.Malfunction,
        //        SerialNumber = this.SerialNumber,
        //        DateOfRaceipt = DateTime.Now,
        //        SumMoney = 0,
        //        UserID = user.UserId
        //    };
        //    unitOfWork.Repairs.AddElemet(rapair);            
        //}
        public ICommand Order
        {
            get => new DelegateCommand((obj) =>
            {
                if(!CheckNotNull(typeof(string), Device, SerialNumber, SelectService))
                {
                    ViewManager veiw = ViewManager.GetInstance;
                    Rapair rapair = new Rapair
                    {
                        UserID = veiw.User.UserId,
                        SerialNumber = this.SerialNumber,
                        Device = this.Device,
                        SumMoney = SelectService.Price,
                        CompletedWorks = SelectService.ServiceId,
                        DateOfRaceipt = DateTime.Now,
                        Status = StatusEnum.WaitingDiagnosis.ToString()
                    };
                    unitOfWork.Repairs.AddElemet(rapair);
                    unitOfWork.Save();
                }
                else
                {
                    MessageBox.Show("Заполните все поля");
                }
            });
        }
        public string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
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
      
    }
}
