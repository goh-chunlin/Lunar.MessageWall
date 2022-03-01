using MessageWebAPI.Models;

namespace MessageWebAPI.Repositories
{
    public interface ICoreRepository
    {
        Task<List<Message>> GetMessagesAsync();

        Task<bool> InsertMessageAsync(Message customer);

        Task<bool> DeleteMessageAsync(int id);
    }
}
