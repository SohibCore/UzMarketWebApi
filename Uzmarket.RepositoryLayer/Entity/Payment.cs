using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UzMarket.Core;

namespace UzMarket.RepositoryLayer.Entity
{
    [Table("SYS_PAYMENT")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("ORDER_ID")]
        public long OrderId { get; set; }

        [Column("AMOUNT")]
        public decimal Amount { get; set; }

        [Column("PAYMENT_METHOD_ID")]
        public int PaymentMethodId { get; set; }

        [Column("STATUS_ID")]
        public int OrderStatusId { get; set; }

        [Column("TRANSACTION_DATE")]
        public DateTime TransactionDate { get; set; }

        [Column("CREATED_USER_ID")]
        public int? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; set; }

        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus OrderStatus { get; set; }
    }
}
