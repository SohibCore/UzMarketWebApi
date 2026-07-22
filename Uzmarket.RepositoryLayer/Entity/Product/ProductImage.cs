using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_PRODUCT_IMAGE")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("IMAGE_URL")]
        public string ImageUrl { get; set; } = null!;

        [Column("MAIN_PIC")]
        public bool MainPic { get; set; }

        [Column("SORT_ORDER")]
        public int SortOrder { get; set; } //Tartib raqam
    }
}
