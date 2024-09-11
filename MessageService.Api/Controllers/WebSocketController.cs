using MessageService.Api.BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageService.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketBL _webSocketBL;
        private readonly ILogger<WebSocketController> _logger;

        public WebSocketController(IWebSocketBL webSocketBL, ILogger<WebSocketController> logger)
        {
            _webSocketBL = webSocketBL;
            _logger = logger;
        }

        [Route("ws")]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    _logger.LogInformation("WebSocket request received");
                    await _webSocketBL.HandleWebSocket(HttpContext);
                    _logger.LogInformation("WebSocket request handled successfully");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Invalid WebSocket request");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling WebSocket request");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
