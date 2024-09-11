using MessageService.Api.BL;
using MessageService.Api.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageServiceTest
{
    public class WebSocketBLTests
    {
        private readonly WebSocketBL _webSocketBL;

        public WebSocketBLTests()
        {
            _webSocketBL = new WebSocketBL();
        }

        [Fact]
        public async Task BroadcastMessage_SendsMessageToAllOpenSockets()
        {
            // Arrange
            var message = new Message { Id = 1, Text = "Test Message" };
            var socketMock = new Mock<WebSocket>();
            socketMock.Setup(s => s.State).Returns(WebSocketState.Open);
            socketMock.Setup(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<WebSocketMessageType>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

            _webSocketBL.GetType().GetField("_sockets", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .SetValue(_webSocketBL, new List<WebSocket> { socketMock.Object });

            // Act
            await _webSocketBL.BroadcastMessage(message);

            // Assert
            socketMock.Verify(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<WebSocketMessageType>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
