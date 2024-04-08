namespace Invoicing.DTOs
{
    public class ItemDTO
    {
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
        public string Hash { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
