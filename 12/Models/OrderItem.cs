using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreApp.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Навигационные свойства
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }

        // Вычисляемое свойство (не сохраняется в БД)
        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}