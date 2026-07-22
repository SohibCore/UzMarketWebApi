using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_PRODUCT")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("NAME")]
        public string Name { get; set; } = null!;

        [Column("DESCRIPTION")]
        public string Description { get; set; } = null!;

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("STOCK_QUANTITY")]
        public int StockQuantity { get; set; }

        [Column("CATEGORY_ID")]
        public long CategoryId { get; set; }

        [Column("SUPPLIER_ID")]
        public long SupplierId { get; set; } //UserId

        [Column("STATUS_ID")]
        public int StatusId { get; set; }

        [Column("CREATED_USER_ID")]
        public long? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public long? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;

        [ForeignKey(nameof(SupplierId))]
        public User User { get; set; } = null!;
        public StatusIdConst Status { get; set; }

        public ICollection<ProductImage> Tables { get; set; } = new List<ProductImage>();
    }
}
