using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_CART_ITEM")]
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("CART_ID")]
        public long CartId { get; set; }

        [Column("PRODUCT_ID")]
        public long ProductId { get; set; }

        [Column("QUANTITY")]
        public int Quantity { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
