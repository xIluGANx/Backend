using WebAPIApp.Models;

namespace WebAPIApp.Repositories
{
    /// <summary>
    /// Реализация репозитория товаров с хранением в памяти
    /// В реальном приложении здесь будет работа с базой данных
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        // Коллекция для хранения товаров в памяти
        private readonly List<Product> _products;

        // Счетчик для генерации уникальных идентификаторов
        private int _nextId = 1;

        /// <summary>
        /// Конструктор репозитория
        /// Инициализирует коллекцию начальными данными
        /// </summary>
        public ProductRepository()
        {
            _products = new List<Product>
            {
                new Product {
                    Id = _nextId++,
                    Name = "Ноутбук",
                    Price = 50000,
                    Category = "Электроника",
                    Description = "Мощный ноутбук для работы и игр",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    Id = _nextId++,
                    Name = "Смартфон",
                    Price = 25000,
                    Category = "Электроника",
                    Description = "Современный смартфон с отличной камерой",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    Id = _nextId++,
                    Name = "Книга по программированию",
                    Price = 1500,
                    Category = "Книги",
                    Description = "Учебное пособие по C# и .NET",
                    CreatedDate = DateTime.Now
                }
            };
        }

        /// <summary>
        /// Получить все товары
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            // Возвращаем копию коллекции для избежания изменений извне
            return _products.ToList();
        }

        /// <summary>
        /// Получить товар по идентификатору
        /// </summary>
        public Product GetById(int id)
        {
            // Используем FirstOrDefault для безопасного поиска
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        public void Add(Product product)
        {
            // Проверка входного параметра
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Генерируем новый ID и устанавливаем дату создания
            product.Id = _nextId++;
            product.CreatedDate = DateTime.Now;

            // Добавляем товар в коллекцию
            _products.Add(product);
        }

        /// <summary>
        /// Обновить существующий товар
        /// </summary>
        public void Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Находим существующий товар
            var existingProduct = GetById(product.Id);
            if (existingProduct != null)
            {
                // Обновляем все свойства, кроме ID и даты создания
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Category = product.Category;
                existingProduct.Description = product.Description;
            }
        }

        /// <summary>
        /// Удалить товар по идентификатору
        /// </summary>
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        /// <summary>
        /// Проверить существование товара по идентификатору
        /// </summary>
        public bool Exists(int id)
        {
            return _products.Any(p => p.Id == id);
        }
    }
}