using MessageService.Api.Models;

namespace MessageService.Api.BL
{
    public interface IMessageBL
    {
        Task<int> SendMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessageAsync(DateTime startDate, DateTime endDate);
    }
}
