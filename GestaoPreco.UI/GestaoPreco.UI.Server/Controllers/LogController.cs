using GestaoPedido.Infrastructure.DTO;
using GestaoPedido.Infrastructure.Repository;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedido.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IErrorLogRepository _repository;

        public LogController(IErrorLogRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] LogFilter filter)
        {
            var logs = await _repository.SearchAsync(filter);

            return Ok(logs);
        }
    }
}