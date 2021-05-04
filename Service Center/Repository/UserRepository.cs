using Service_Center.Contexts;
using Service_Center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    class UserRepository : IRepository<User>
    {
        readonly Context context;
        public UserRepository(Context context) => this.context = context;
        public UserRepository() => this.context = new Context();
        /// <summary>
        /// Добавляет элемент в context
        /// Используйте метод Save для сохранения в бд
        /// </summary>
        /// <param name="item"></param>
        public void AddElemet(User item)
        {
            if (item != null)
                context.Users.Add(item);
        }
        /// <summary>
        /// Удаление элемента по первичному ключу
        /// </summary>
        /// <param name="id"></param> 
        public void Delete(int id)
        {
            User User = context.Users.Find(id);
            if (User != null)
                context.Users.Remove(User);
        }
        /// <summary>
        /// Получает элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetItem(int id)
        {
            return context.Users.Find(id);
        }
        /// <summary>
        /// Получить весь список элементов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetItemList()
        {
            return context.Users.ToArray();
        }
        /// <summary>
        /// Обновляет состояние объекта Entity
        /// </summary>
        /// <param name="item"></param>
        public void Update(User item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Возвращает первый элемент коллекции
        /// </summary>
        /// <returns></returns>
        public User GetFirstItem()
        {
            return context.Users.First();
        }
    }
}
