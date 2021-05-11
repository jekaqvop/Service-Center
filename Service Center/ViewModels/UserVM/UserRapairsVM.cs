using Service_Center.Commands;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rapair_Center.ViewModels.UserVM
{
    class UserRapairsVM : PropertysChanged
    {
        public UserRapairsVM()
        {
            sortRapairsList();
        }
        UnitOfWork unitOfWork = new UnitOfWork();
        public ObservableCollection<Rapair> RapairsList { get; set; }
        #region selectingByPrice
        decimal price0 = 0;
        decimal price1 = 9999999;
        public decimal Price0
        {
            get
            {
                sortRapairsList();
                return price0;
            }
            set => price0 = value;
        }
        public decimal Price1
        {
            get
            {
                sortRapairsList();
                return price1;
            }
            set => price1 = value;
        }

        #endregion
        #region ChangeSelectRapair
        public int SelectIndex { get; set; }
       
        public ICommand Update
        {
            get => new DelegateCommand((obj) =>
            {
                sortRapairsList();
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
                sortRapairsList();
                return sortAsc;
            }
            set => sortAsc = value;
        }
        public string SortDesc
        {
            get
            {
                sortRapairsList();
                return sortDesc;
            }
            set => sortDesc = value;
        }
        public string SortTitle
        {
            get
            {
                sortRapairsList();
                return sortTitle;
            }
            set => sortTitle = value;
        }
        public string SortPrice
        {
            get
            {
                sortRapairsList();
                return sortPrice;
            }
            set => sortPrice = value;
        }
        public string SearchTitle
        {
            get
            {
                sortRapairsList();
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
        void sortRapairsList()
        {
            ViewController view = ViewController.GetInstance;
            unitOfWork.Save();
            switch (getSortParam())
            {
                case "TASC":
                    RapairsList = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList()
                        .Where(p => p.SumMoney >= price0 && p.SumMoney <= price1 
                        && (p.Malfunction + p.SerialNumber + p.Device).Contains(searchTitle)
                        && view.User.UserId == p.UserID).OrderBy(p => p.Malfunction));
                    break;
                case "PASC":
                    RapairsList = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(p => p.SumMoney >= price0 
                    && p.SumMoney <= price1 
                    && (p.Malfunction + p.SerialNumber + p.Device).Contains(searchTitle)
                    && view.User.UserId == p.UserID).OrderBy(p => p.SumMoney));
                    break;
                case "TDESC":
                    RapairsList = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(p => p.SumMoney >= price0 
                    && p.SumMoney <= price1 
                    && (p.Malfunction + p.SerialNumber + p.Device).Contains(searchTitle)
                    && view.User.UserId == p.UserID).OrderByDescending(p => p.Malfunction));
                    break;
                case "PDESC":
                    RapairsList = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(p => p.SumMoney >= price0 
                    && p.SumMoney <= price1 
                    && (p.Malfunction + p.SerialNumber + p.Device).Contains(searchTitle)
                    && view.User.UserId == p.UserID).OrderByDescending(p => p.SumMoney));
                    break;
                default:
                    RapairsList = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(p => p.SumMoney >= price0 
                    && p.SumMoney <= price1 
                    && (p.Malfunction + p.SerialNumber + p.Device).Contains(searchTitle)
                    && view.User.UserId == p.UserID));
                    break;
            }
        }
        #endregion
    }
}
