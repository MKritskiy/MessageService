using MessageService.Api.DAL;
using MessageService.Api.Models;

namespace MessageService.Api.BL
{
    public class MessageBL : IMessageBL
    {
        private readonly IMessageDAL _messageDAL;
        private readonly IWebSocketBL _websocketBL;
        private readonly ILogger<MessageBL> _logger;

        public MessageBL(IMessageDAL messageDAL, IWebSocketBL websocketBL, ILogger<MessageBL> logger)
        {
            _messageDAL = messageDAL;
            _websocketBL = websocketBL;
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> GetMessageAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Getting messages from {StartDate} to {EndDate}", startDate, endDate);
                var messages = await _messageDAL.GetMessagesAsync(startDate, endDate);
                _logger.LogInformation("Retrieved {MessageCount} messages", messages.Count());
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages from {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<int> SendMessageAsync(Message message)
        {
            try
            {
                _logger.LogInformation("Saving message: {MessageText}", message.Text);
                message.Timestamp = DateTime.Now;
                int messageId = await _messageDAL.SaveMessageAsync(message);
                message.Id = messageId;
                await _websocketBL.BroadcastMessage(message);
                _logger.LogInformation("Message saved successfully with ID: {MessageId}", messageId);
                return messageId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving message: {MessageText}", message.Text);
                throw;
            }
        }
    }
}
