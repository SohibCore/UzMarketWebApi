using UzMarket.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_REVIEW")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public int Id { get; set; }

        [Column("PRODUCT_ID")]
        public long ProductId { get; set; }

        [Column("USER_ID")]
        public long UserId { get; set; }

        [Column("RATING")]
        public int RatingId { get; set; }

        [Column("COMMENT")]
        public string? Comment { get; set; }

        [Column("STATUS_ID")]
        public StatusIdConst StatusId { get; set; }

        [Column("CREATED_USER_ID")]
        public long? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public long? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(RatingId))]
        public Rating Rating { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
