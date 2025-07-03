using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestaoPreco.Application.Queries.OrderItem;
using GestaoPreco.Application.Commands.OrderItem;

namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItem>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _mediator.Send(new ListOrderItemsQuery());
                return Ok(items);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar itens do pedido.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderItem), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _mediator.Send(new GetOrderItemByIdQuery { Id = id });
                if (item == null)
                    return NotFound();
                return Ok(item);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar item do pedido.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderItem), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var itemId = await _mediator.Send(command);
                if (itemId == Guid.Empty)
                    return BadRequest();

                var item = await _mediator.Send(new GetOrderItemByIdQuery { Id = itemId });
                return CreatedAtAction(nameof(GetById), new { id = itemId }, item);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao criar item do pedido.");
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderItemCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                command.Id = id;
                var success = await _mediator.Send(command);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao atualizar item do pedido.");
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteOrderItemCommand(id));
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao deletar item do pedido.");
            }
        }
    }
}