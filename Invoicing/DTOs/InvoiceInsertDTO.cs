namespace Invoicing.DTOs
{
    public class InvoiceInsertDTO
    {
        public string? Hash { get; set; }
        public int ClientId { get; set; }
        public decimal Total { get; set; }
    }
}
