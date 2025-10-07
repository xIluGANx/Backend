namespace WebAPIApp.Models
{
    /// <summary>
    /// Модель данных для товара
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Уникальный идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Описание товара
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}