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
        Task<IEnumerable<T>> GetItemsListAsync(); // получение всех объектов
        Task<T> GetItemAsync(int id); // получение одного объекта по id
        Task CreateAsync(T item); // создание объекта
        Task UpdateAsync(T item); // обновление объекта
        Task DeleteAsync(int id); // удаление объекта по id
        Task SaveAsync();  // сохранение изменений
        Task<IEnumerable<T>> GetItemsListAsync(int id); // получить навигационное свойство-список обьектов по Id родительского элемента  
    }
}
