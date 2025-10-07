using WebAPIApp.Models;

namespace WebAPIApp.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с товарами
    /// Определяет контракт для операций CRUD (Create, Read, Update, Delete)
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Получить все товары
        /// </summary>
        /// <returns>Коллекция всех товаров</returns>
        IEnumerable<Product> GetAll();

        /// <summary>
        /// Получить товар по идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор товара</param>
        /// <returns>Найденный товар или null</returns>
        Product GetById(int id);

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="product">Объект товара для добавления</param>
        void Add(Product product);

        /// <summary>
        /// Обновить существующий товар
        /// </summary>
        /// <param name="product">Объект товара с обновленными данными</param>
        void Update(Product product);

        /// <summary>
        /// Удалить товар по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара для удаления</param>
        void Delete(int id);

        /// <summary>
        /// Проверить существование товара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>True если товар существует, иначе False</returns>
        bool Exists(int id);
    }
}