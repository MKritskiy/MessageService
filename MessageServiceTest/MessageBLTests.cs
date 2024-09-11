using MessageService.Api.BL;
using MessageService.Api.DAL;
using MessageService.Api.Models;
using Moq;
using Xunit;

public class MessageBLTests
{
    private readonly Mock<IMessageDAL> _messageDALMock;
    private readonly Mock<IWebSocketBL> _websocketBLMock;
    private readonly MessageBL _messageBL;

    public MessageBLTests()
    {
        _messageDALMock = new Mock<IMessageDAL>();
        _websocketBLMock = new Mock<IWebSocketBL>();
        _messageBL = new MessageBL(_messageDALMock.Object, _websocketBLMock.Object);
    }

    [Fact]
    public async Task GetMessageAsync_ReturnsMessagesFromDAL()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var expectedMessages = new List<Message>
        {
            new Message { Id = 1, Text = "Message 1" },
            new Message { Id = 2, Text = "Message 2" }
        };
        _messageDALMock.Setup(dal => dal.GetMessagesAsync(startDate, endDate))
                       .ReturnsAsync(expectedMessages);

        // Act
        var result = await _messageBL.GetMessageAsync(startDate, endDate);

        // Assert
        Assert.Equal(expectedMessages, result);
    }

    [Fact]
    public async Task SendMessageAsync_SavesMessageAndBroadcasts()
    {
        // Arrange
        var message = new Message { Text = "Test Message" };
        var messageId = 1;
        _messageDALMock.Setup(dal => dal.SaveMessageAsync(It.IsAny<Message>()))
                       .ReturnsAsync(messageId);

        // Act
        var result = await _messageBL.SendMessageAsync(message);

        // Assert
        _messageDALMock.Verify(dal => dal.SaveMessageAsync(It.IsAny<Message>()), Times.Once);
        _websocketBLMock.Verify(ws => ws.BroadcastMessage(It.IsAny<Message>()), Times.Once);
        Assert.Equal(messageId, result);
        Assert.NotNull(message.Timestamp);
        Assert.Equal(messageId, message.Id);
    }
}
