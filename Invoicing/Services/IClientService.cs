namespace Invoicing.Services
{
    public interface IClientService<T, TI, TU>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetByMat(string mat);

        Task<T> Add(TI entityInsertDTO);
        Task<T> Update(string mat, TU entityUpdateDTO);
        Task<T> Delete(string mat);
        bool Validate(TI entityInsertDTO);
        bool Validate(TU entityUpdateDTO);
        public List<string> Errors { get; }
    }
}
