using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_CATEGORY")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("NAME")]
        public string Name { get; set; } = null!;

        [Column("DESCRIPTION")]
        public string Description { get; set; } = null!;

        [Column("PARENT_CATEGORY_ID")]
        public long? ParentCategoryId { get; set; }

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

        public List<Category> ChildCategories { get; set; } = new List<Category>();

        //Navigation property
        public Category? ParentCategory { get; set; }
        public StatusIdConst Status { get; set; }
    }
}
