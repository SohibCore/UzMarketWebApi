using System.ComponentModel.DataAnnotations.Schema;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_FAVORITE")]
    public class Favorite
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("USER_ID")]
        public long UserId { get; set; }

        [Column("PRODUCT_ID")]
        public long ProductId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
