using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoicing.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
        //public int InvoiceID { get; set; }

        public string Hash { get; set; }
        [ForeignKey("Hash")]
        public virtual Invoice Invoice { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
