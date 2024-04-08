namespace Invoicing.DTOs
{
    public class InvoiceDTO
    {
        public string? Hash { get; set; }
        public DateTime date { get; set; }
        public int ClientId { get; set; }
        public decimal Total { get; set; }
    }
}
