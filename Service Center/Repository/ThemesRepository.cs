using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Repository
{
    class ThemesRepository : IRepository<Themes>
    {
        readonly Context context;
        public ThemesRepository(Context context) => this.context = context;
        public ThemesRepository() => this.context = new Context();
        /// <summary>
        /// Добавляет элемент в context
        /// Используйте метод Save для сохранения в бд
        /// </summary>
        /// <param name="item"></param>
        public void AddElemet(Themes item)
        {
            if (item != null)
                context.Themes.Add(item);
        }
        /// <summary>
        /// Удаление элемента по первичному ключу
        /// </summary>
        /// <param name="id"></param> 
        public void Delete(int id)
        {
            Themes Theme = context.Themes.Find(id);
            if (Theme != null)
                context.Themes.Remove(Theme);
        }
        /// <summary>
        /// Получает элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Themes GetItem(int id)
        {
            return context.Themes.Find(id);
        }
        /// <summary>
        /// Получить весь список
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Themes> GetItemList()
        {
            return context.Themes.ToArray();
        }
        /// <summary>
        /// Обновляет состояние объекта Entity
        /// </summary>
        /// <param name="item"></param>
        public void Update(Themes item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Получение первого элемента
        /// </summary>
        /// <returns></returns>
        public Themes GetFirstItem()
        {
            return context.Themes.First();
        }
    }
}
