namespace Invoicing.Services
{
    public interface ICommonService<T, TI, TU>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetByName(string name);


        Task<T> GetById(int id);
        Task<T> Add(TI entityInsertDTO);
        Task<T> Update(int id, TU entityUpdateDTO);
        Task<T> Delete(int id);
        bool Validate(TI entityInsertDTO);
        bool Validate(TU entityUpdateDTO);
        public List<string> Errors { get; }
    }
}
