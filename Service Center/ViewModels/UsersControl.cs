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
                    Collection<object> objects = (Collection<object>)obj;
                    List<User> list = objects.Cast<User>().ToList();
                    list.ForEach(user =>
                    {
                        unitOfWork.Repairs.Delete(user.UserId);
                        ListUsers.Remove(user);
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
                unitOfWork = new UnitOfWork();
                ListUsers = new ObservableCollection<User>(unitOfWork.Users.GetItemList());
            });
        }
    }
}
