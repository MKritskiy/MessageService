using MessageService.Api.Models;
using System.Net.WebSockets;

namespace MessageService.Api.BL
{
    public interface IWebSocketBL
    {
        Task HandleWebSocket(HttpContext context);
        Task ReceiveMessage(WebSocket socket);
        Task BroadcastMessage(Message message);
    }
}
