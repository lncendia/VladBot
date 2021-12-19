namespace VladBot.Core.Interfaces;

public interface IRepository<T>
{
    public List<T> GetAll();
    public void Add(T entity);
    public void Delete(T entity);
    public void Update(T entity);
    public T? Get(long id);
}