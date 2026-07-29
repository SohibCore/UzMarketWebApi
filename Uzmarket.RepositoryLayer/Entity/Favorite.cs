using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_FAVORITE")]
    public class Favorite
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_ID")]
        public long UserId { get; set; }

        [Column("PRODUCT_ID")]
        public long ProductId { get; set; }

        [Column("STATUS_ID")]
        public StatusIdConst StatusId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
