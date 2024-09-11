using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MessageService.Api.Controllers
{
    /// <summary>
    /// Контроллер для обслуживания статических HTML-страниц.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;

        public PagesController(ILogger<PagesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Возвращает страницу перехватчика сообщений.
        /// </summary>
        /// <returns>Страница перехватчика сообщений.</returns>
        /// <response code="200">OK. Возвращает страницу перехватчика сообщений.</response>
        [HttpGet("handler")]
        [SwaggerResponse(200, "OK", typeof(ViewResult))]
        public IActionResult HandlerPage()
        {
            _logger.LogInformation("Handler page requested");
            return View("Handler");
        }

        /// <summary>
        /// Возвращает страницу отправителя сообщений.
        /// </summary>
        /// <returns>Страница отправителя сообщений.</returns>
        /// <response code="200">OK. Возвращает страницу отправителя сообщений.</response>
        [HttpGet("sender")]
        [SwaggerResponse(200, "OK", typeof(ViewResult))]
        public IActionResult SenderPage()
        {
            _logger.LogInformation("Sender page requested");
            return View("Sender");
        }

        /// <summary>
        /// Возвращает страницу истории.
        /// </summary>
        /// <returns>Страница истории сообщений.</returns>
        /// <response code="200">OK. Возвращает страницу истории сообщений.</response>
        [HttpGet("history")]
        [SwaggerResponse(200, "OK", typeof(ViewResult))]
        public IActionResult HistoryPage()
        {
            _logger.LogInformation("History page requested");
            return View("History");
        }
    }
}
