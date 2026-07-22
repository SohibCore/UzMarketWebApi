using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_ORDER_ITEM")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("ORDER_ID")]
        public long OrderId { get; set; }

        [Column("PRODUCT_ID")]
        public long ProductId { get; set; }

        [Column("QUANTITY")]
        public int Quantity { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("CREATED_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}
