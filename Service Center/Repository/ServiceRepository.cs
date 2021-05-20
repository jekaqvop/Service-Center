using Service_Center.Contexts;
using Service_Center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    class ServiceRepository : IRepository<Service>
    {
        readonly Context context;
        public ServiceRepository(Context context) => this.context = context;      
        public ServiceRepository() => this.context = new Context();
        /// <summary>
        /// Добавляет элемент в context
        /// Используйте метод Save для сохранения в бд
        /// </summary>
        /// <param name="item"></param>
        public void AddElemet(Service item)
        {
            if(item != null)
                context.Services.Add(item);
        }
        /// <summary>
        /// Удаление элемента по первичному ключу
        /// </summary>
        /// <param name="id"></param> 
        public void Delete(int id)
        {
            //Service service = context.Services.Find(id);
            //if(service != null)
            //    context.Services.Remove(service);
        }
        /// <summary>
        /// Получает элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Service GetItem(int id)
        {
            //return context.Services.Find(id);
            return null;
        }
        /// <summary>
        /// Получить весь список
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Service> GetItemList()
        {
            if (context.Services.Where(p => p.Title == "Нет").Count() <= 0)
            {
                context.Services.Add(new Service { Title = "Нет", Info = "" });
                context.SaveChanges();
            }                
            return context.Services.ToArray();
        }
        /// <summary>
        /// Обновляет состояние объекта Entity
        /// </summary>
        /// <param name="item"></param>
        public void Update(Service item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Получение первого элемента
        /// </summary>
        /// <returns></returns>
        public Service GetFirstItem()
        {
            return context.Services.First();
        }
    }
}
