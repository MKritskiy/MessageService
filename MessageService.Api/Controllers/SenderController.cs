using MessageService.Api.BL;
using MessageService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MessageService.Api.Controllers
{
    /// <summary>
    /// Контроллер отправителя сообщений.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly IMessageBL _messageBL;
        private readonly ILogger<SenderController> _logger;

        public SenderController(IMessageBL messageBL, ILogger<SenderController> logger)
        {
            _messageBL = messageBL;
            _logger = logger;
        }
        /// <summary>
        /// Отправляет новое сообщение.
        /// </summary>
        /// <param name="message">Сообщение для отправки.</param>
        /// <returns>OK, если сообщение успешно отправлено.</returns>
        /// <response code="200">OK. Сообщение успешно отправлено.</response>
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] Message message)
        {
            try
            {
                _logger.LogInformation("Sending message: {MessageText}", message.Text);
                await _messageBL.SendMessageAsync(message);
                _logger.LogInformation("Message sent successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message: {MessageText}", message.Text);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
