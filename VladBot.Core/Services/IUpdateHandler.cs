namespace VladBot.Core.Services;

public interface IUpdateHandler<in T>
{
    public Task HandleAsync(T update);
    public void HandleErrorAsync(T update, Exception ex);
}