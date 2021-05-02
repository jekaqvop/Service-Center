using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получить все элементы
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetItemList(); 
        /// <summary>
        /// Получить элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetItem(int id); 
        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="item"></param>
        void AddElemet(T item);
        /// <summary>
        /// Изменить состояние объекта на радактируемое
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);
        /// <summary>
        /// Удалить элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id); // удаление объекта по id
        /// <summary>
        /// Позволяет получить первый элемент
        /// </summary>
        /// <returns></returns>
        T GetFirstItem();
    }
}
