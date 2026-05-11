using GestaoPedido.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPreco.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly IAiPricingService _service;

    public PricingController(IAiPricingService service)
    {
        _service = service;
    }

    /// <summary>
    /// Analisa o preço de um produto com IA (Groq). Query opcional: product, currentPrice (padrão: Notebook Gamer, 4500).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Analyze([FromQuery] string product = "Notebook Gamer",[FromQuery] decimal currentPrice = 4500)
    {
        var result = await _service.AnalyzePrice(product, currentPrice);
        return Ok(result);
    }
}
