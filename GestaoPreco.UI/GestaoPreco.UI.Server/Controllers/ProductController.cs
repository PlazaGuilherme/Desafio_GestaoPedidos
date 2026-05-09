using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Queries.Product;
using GestaoPreco.Application.Commands.Product;

namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _mediator.Send(
                new ListProductsQuery());

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _mediator.Send(
                new GetProductByIdQuery
                {
                    Id = id
                });

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _mediator.Send(command);

            if (productId == Guid.Empty)
                return BadRequest();

            return CreatedAtAction(
                nameof(GetById),
                new { id = productId },
                command);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(
                new DeleteProductCommand(id));

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}