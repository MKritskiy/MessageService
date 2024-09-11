using MessageService.Api.BL;
using MessageService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MessageService.Api.Controllers
{
    /// <summary>
    /// Контроллер истории сообщений.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IMessageBL _messageBL;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(IMessageBL messageBL, ILogger<HistoryController> logger)
        {
            _messageBL = messageBL;
            _logger = logger;
        }
        /// <summary>
        /// Получает сообщения за указанный период времени.
        /// </summary>
        /// <param name="startDate">Начальная дата периода.</param>
        /// <param name="endDate">Конечная дата периода.</param>
        /// <returns>Список сообщений за указанный период времени.</returns>
        /// <response code="200">OK. Возвращает список сообщений.</response>
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Message>))]
        public async Task<IActionResult> GetMessages([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Getting messages from {StartDate} to {EndDate}", startDate, endDate);
                var messages = await _messageBL.GetMessageAsync(startDate, endDate);
                _logger.LogInformation("Retrieved {MessageCount} messages", messages.Count());
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages from {StartDate} to {EndDate}", startDate, endDate);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
