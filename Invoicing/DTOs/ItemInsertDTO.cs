namespace Invoicing.DTOs
{
    public class ItemInsertDTO
    {
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
        public string Hash { get; set; }
        public int ProductId { get; set; }
    }
}
