using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contenter.Infrastructure.Repository.DI.Abstract
{
    public interface IRepository<T>:IDisposable
         where T : class
    {
        IEnumerable<T> GetItemsList(); // получение всех объектов
        T GetItem(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
        IEnumerable<T> GetItemsList(int id); // получить навигационное свойство-список обьектов по Id родительского элемента  
    }
}
