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
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class UsersControlVM : PropertysChanged
    {
        UnitOfWork unitOfWork;
        public ObservableCollection<User> ListUsers { get; set; } = new ObservableCollection<User>();
        public UsersControlVM() 
        {
            unitOfWork = new UnitOfWork();
            ListUsers = new ObservableCollection<User>(unitOfWork.Users.GetItemList());
        }
        public User User { get; set; }
        /// <summary>
        /// Сохранение изменений в бд
        /// </summary>
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                unitOfWork.Save();
            });
        }
        /// <summary>
        /// Удаление выбранных пользователей
        /// </summary>
        public ICommand DeleteRows
        {
            get => new DelegateCommand((obj) =>
            {
                if (((Collection<object>)obj).Count > 0)
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("При удалении пользователя все, связанные с ним заказы, будут удалены!\n" +
                             "Вы уверены, что хотите удалить пользователя и все, связанные с ним заказы?",
                                         "Нужный пользователь отсутствует",
                                         MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Collection<object> objects = (Collection<object>)obj;
                        List<User> list = objects.Cast<User>().ToList();
                        list.ForEach(user =>
                        {
                            IEnumerable<Rapair> rapairs = unitOfWork.Repairs.GetItemList().Where(r => r.UserID == user.UserId);
                            foreach(Rapair r in rapairs)
                            {
                                unitOfWork.Repairs.Delete(r.RapairID);
                            }
                            unitOfWork.Repairs.Delete(user.UserId);
                            ListUsers.Remove(user);
                        });
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Удаление пользователя прервано!");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Не выбраны элементы для удаления");
            });
        }

        public ICommand UpdateDataGrid
        {
            get => new DelegateCommand((obj) =>
            {
                unitOfWork = new UnitOfWork();
                ListUsers = new ObservableCollection<User>(unitOfWork.Users.GetItemList());
            });
        }
        public ICommand ShowFullInfo
        {
            get => new DelegateCommand((obj) =>
            {
                
            });
        }
    }
}
