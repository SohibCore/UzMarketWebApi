using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_ORDER")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("USER_ID")]
        public long UserId { get; set; }

        [Column("ORDER_DATE")]
        public string OrderDate { get; set; } = null!;

        [Column("TOTAL_AMOUNT")]
        public decimal TotalAmount { get; set; }

        [Column("ORDER_STATUS_ID")]
        public int OrderStatusId { get; set; }

        [Column("SHIPPING_ADDRESS_ID")]
        public int ShippingAddressId { get; set; }

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
        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus Status { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(ShippingAddressId))]
        public Address Address { get; set; } = null!;

        [ForeignKey(nameof(StatusId))]
        public StatusIdConst StatusConst { get; set; }

        public ICollection<OrderItem> Tables { get; set; } = new List<OrderItem>();
    }
}
