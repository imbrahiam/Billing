namespace Invoicing.Services
{
    public interface IItemService<T, TI>
    {
        Task<IEnumerable<T>> GetAllByHash(string hash);

        Task<IEnumerable<T>> Add(IEnumerable<TI> entitiesInsertDTO);
        bool Validate(IEnumerable<TI> entitiesInsertDTO);
        List<string> Errors { get; }
    }
}
