namespace Invoicing.Services
{
    public interface IInvoiceService<T, TI>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetByHash(string hash);
        Task<T> Add(TI entityInsertDTO);
        bool Validate(TI entityInsertDTO);
        public List<string> Errors { get; }
    }
}
