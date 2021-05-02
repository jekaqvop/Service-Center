using Service_Center.Contexts;
using Service_Center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    class RapairRepository : IRepository<Rapair>
    {
        readonly Context context;
        public RapairRepository(Context context) => this.context = context;
        public RapairRepository() => this.context = new Context();
        /// <summary>
        /// Добавляет элемент в context
        /// Используйте метод Save для сохранения в бд
        /// </summary>
        /// <param name="item"></param>
        public void AddElemet(Rapair item)
        {
            if (item != null)
                context.Rapairs.Add(item);
        }
        /// <summary>
        /// Удаление элемента по первичному ключу
        /// </summary>
        /// <param name="id"></param> 
        public void Delete(int id)
        {
            Rapair Rapair = context.Rapairs.Find(id);
            if (Rapair != null)
                context.Rapairs.Remove(Rapair);
        }
        /// <summary>
        /// Получает элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rapair GetItem(int id)
        {
            return context.Rapairs.Find(id);
        }
        /// <summary>
        /// Получить весь список
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rapair> GetItemList()
        {
            return context.Rapairs.ToArray();
        }
        /// <summary>
        /// Обновляет состояние объекта Entity
        /// </summary>
        /// <param name="item"></param>
        public void Update(Rapair item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Получение первого элемента
        /// </summary>
        /// <returns></returns>
        public Rapair GetFirstItem()
        {
            return context.Rapairs.First();
        }
    }
}
