using MessageService.Api.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageService.Api.BL
{
    public class WebSocketBL : IWebSocketBL
    {
        private readonly List<WebSocket> _sockets = new List<WebSocket>();
        private readonly ILogger<WebSocketBL> _logger;

        public WebSocketBL(ILogger<WebSocketBL> logger)
        {
            _logger = logger;
        }

        public async Task BroadcastMessage(Message message)
        {
            try
            {
                var messageJson = JsonSerializer.Serialize(message);
                var messageBytes = Encoding.UTF8.GetBytes(messageJson);
                foreach (var socket in _sockets)
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        _logger.LogInformation("Broadcasting message to socket");
                        await socket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting message");
            }
        }

        public async Task HandleWebSocket(HttpContext context)
        {
            try
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.Add(webSocket);
                _logger.LogInformation("WebSocket connection established");
                await ReceiveMessage(webSocket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling WebSocket connection");
            }
        }

        public async Task ReceiveMessage(WebSocket socket)
        {
            try
            {
                var buffer = new byte[1024 * 4];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    _logger.LogInformation("Message received from socket");
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                _logger.LogInformation("Closing WebSocket connection");
                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                _sockets.Remove(socket);
                _logger.LogInformation("WebSocket connection closed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving message from WebSocket");
            }
        }
    }
}
