using MessageService.Api.Models;

namespace MessageService.Api.DAL
{
    public interface IMessageDAL
    {
        Task<int> SaveMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(DateTime startDate, DateTime endDate);
    }
}
