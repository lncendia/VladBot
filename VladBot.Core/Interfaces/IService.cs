namespace VladBot.Core.Interfaces;

public interface IService<T>
{
    public List<T> GetAll();
    public IOperationResult Delete(T entity);
    public T? Get(long id);
    public void Update(T entity);
    public IOperationResult Add(T item);
}