using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_REVIEW")]
    public class Review
    {
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

        [Column("CREATED_USER_ID")]
        public int? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(RatingId))]
        public Rating Rating { get; set; }
    }
}
