using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    
    class TableRapairVM : PropertysChanged
    {
        public List<string> StatusEnum { get; set; }
        UnitOfWork unitOfWork = new UnitOfWork();
        //Rapair selectRapair;
        public Rapair SelectRapair { get; set; }

        public TableRapairVM()
        {
            Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());    
            SelectRapair = unitOfWork.Repairs.GetFirstItem();
        }        
        public User Rapair { get; set; }
        public string StatusNewRapair { get; set; } = "WaitingDiagnosis";
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Device { get; set; }
        public string Malfunction { get; set; }
        public string SerialNumber { get; set; }
        public ObservableCollection<Rapair> Rapairs { get; set; } = new ObservableCollection<Rapair>();
        #region Command
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                unitOfWork.Save();
            });
        }
        #endregion
        public ICommand DeleteRows
        {
            get => new DelegateCommand((obj) =>
            {
                if (((Collection<object>)obj).Count > 0)
                {
                    Collection<object> objects = (Collection<object>)obj;
                    List<Rapair> list = objects.Cast<Rapair>().ToList();
                    list.ForEach(rapair =>
                    {
                        unitOfWork.Repairs.Delete(rapair.RapairID);
                        Rapairs.Remove(rapair);
                    });
                }
                else
                    MessageBox.Show("Не выбраны элементы для удаления");
            });
        }
        public ICommand UpdateDataGrid
        {
            get => new DelegateCommand((obj) =>
            {
                Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());
            });        
        }
        int searchId = 0;
        public int SearchId
        {
            get => searchId;
            set
            {
                searchId = value;
                if (searchId != 0)
                {                    
                    Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(r => r.RapairID == searchId && r.UserID == searchId));
                    SelectRapair = Rapairs.First();
                }
                else
                {
                    Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());
                    SelectRapair = unitOfWork.Repairs.GetFirstItem();
                }
            }
        }
        public ICommand CreateNewElement
        {
            get => new DelegateCommand((obj) =>
            {
                if(checkNotNull(typeof(string), FullName, StatusNewRapair, PhoneNumber, Device, Malfunction, SerialNumber))
                {
                    IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(p => p.PhoneNumber == PhoneNumber);
                    if (users.Count() == 0)
                    {
                        DialogResult result = MessageBox.Show("Пользователя с таким номером телефона не существует!\n" +
                            "Вы хотите его добавить?",
                                        "Нужный пользователь отсутствует",
                                        MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            User user = new User { PhoneNumber = this.PhoneNumber };
                            ViewController view = ViewController.GetInstance;
                            view.OpenMiniWindow(new RegNewUserWind());
                        }
                        else
                        {
                            MessageBox.Show("В этом случае вы не сможете добавить заказ для несуществующего пользователя!\n" +
                                            "Если пользователь уже зарегестрировался, то проверьте корректность номера телефона!");
                        }
                    }
                    else if (users.Count() == 1)
                    {
                        addRapair(users.First());
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }
               
            });
        }
        public ICommand ShowFullInfo
        {
            get => new DelegateCommand((obj) =>
            {
                User user = unitOfWork.Users.GetItem(SelectRapair.UserID);
                MessageBox.Show($"Номер заказа: {SelectRapair.RapairID}\n" +
                                $"Устройство: {SelectRapair.Device}\n" +
                                $"Несправность: {SelectRapair.Malfunction}\n" +
                                $"ФИО клиента: {user.FullName}\n" +
                                $"Email: {user.Email}\n" +
                                $"Номер телефона: {user.PhoneNumber}");
            });
        }
        bool checkNotNull(Type type, params object[] objects)
        {
            if(type == typeof(string))
            {
                foreach (string obj in objects)
                {
                    if (obj == null || obj == "")
                        return false;
                }
            }
            else
                foreach(object obj in objects)
                {
                    if (obj == null)
                        return false;
                }
            return true;
        }
        void addRapair(User user)
        {
            Rapair rapair = new Rapair
            {
                Status = this.StatusNewRapair,
                Device = this.Device,
                Malfunction = this.Malfunction,
                SerialNumber = this.SerialNumber,
                DateOfRaceipt = DateTime.Now,
                SumMoney = 0,
                UserID = user.UserId
            };
            unitOfWork.Repairs.AddElemet(rapair);
            Rapairs.Add(rapair);
        }
    }
}
