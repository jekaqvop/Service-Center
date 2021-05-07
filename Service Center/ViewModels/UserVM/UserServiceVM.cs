using Service_Center.Commands;
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
                    ServicesList = new ObservableCollection<Service>(unitOfWork.Services.GetItemList().Where(p => p.Price >= price0 && p.Price <= price1 && p.Title.Contains(searchTitle)).OrderBy(p => p.Title));
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
