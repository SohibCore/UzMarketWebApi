using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_USER")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("USER_NAME")]
        [StringLength(100)]
        public string UserName { get; set; } = null!;

        [Column("PASSWORD")]
        [StringLength(300)]
        public string HashPassword { get; set; } = null!;

        [Column("FULL_NAME")]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Column("SHORT_NAME")]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Column("PINFL")]
        [StringLength(14)]
        public string Pinfl { get; set; } = null!;

        [Column("PHONE_NUMBER")]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = null!;

        [Column("ADDRESS")]
        [StringLength(300)]
        public string Address { get; set; } = null!;

        [Column("DATE_OF_BIRTH")]
        [StringLength(10)]
        public string DateOfBirth { get; set; } = null!;

        [Column("PASSPORT_SERIES")]
        [StringLength(9)]
        public string PassportSeries { get; set; } = null!;

        [Column("EMAIL")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Column("STATUS_ID")]
        public int StatusId { get; set; }

        [Column("CREATED_USER_ID")]
        public int? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public StatusIdConst StatusIdConst { get; set; }
    }
}
