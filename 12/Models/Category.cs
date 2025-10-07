using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreApp.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Навигационное свойство для связи один-ко-многим
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}