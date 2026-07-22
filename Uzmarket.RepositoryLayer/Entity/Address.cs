using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_ADDRESS")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_ID")]
        public long UserId { get; set; }

        [Column("REGION")]
        public string Region { get; set; } = null!;

        [Column("CITY")]
        public string City { get; set; } = null!;

        [Column("STREET")]
        public string Street { get; set; } = null!;

        [Column("POSTAL_CODE")]
        public string PostalCode { get; set; } = null!;

        [Column("IS_DIFAULT")]
        public bool IsDefault { get; set; }

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
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public StatusIdConst Status { get; set; }
    }
}
