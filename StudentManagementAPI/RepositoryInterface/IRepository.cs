namespace StudentManagementAPI.RepositoryInterface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(long id);

        Task<ICollection<T>> GetAll();

        Task Remove(long id);

        Task Add(T item);

        Task Update(T item);
    }
}
