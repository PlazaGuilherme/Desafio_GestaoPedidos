using Domain;
using DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestaoPreco.Application.Commands.Customer;
using GestaoPreco.Application.Queries.Customer;

namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerDto[]), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _mediator.Send(new ListCustomersQuery());
                return Ok(customers.Select(CustomerDto.FromEntity));
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar clientes.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
                if (customer == null)
                    return NotFound();
                return Ok(CustomerDto.FromEntity(customer));
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar cliente.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customerId = await _mediator.Send(command);
                if (customerId == Guid.Empty)
                    return BadRequest();

                _logger.LogInformation("Cliente criado: {CustomerId}", customerId);

                // Opcional: buscar o cliente criado para retornar o DTO completo
                var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = customerId });
                return CreatedAtAction(nameof(GetById), new { id = customerId }, CustomerDto.FromEntity(customer));
            }
            catch
            {
                return StatusCode(500, "Erro interno ao criar cliente.");
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                command.Id = id;
                var success = await _mediator.Send(command);
                if (!success)
                    return NotFound();

                _logger.LogInformation("Cliente atualizado: {CustomerId}", id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao atualizar cliente.");
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
                var success = await _mediator.Send(new DeleteCustomerCommand(id));
                if (!success)
                    return NotFound();

                _logger.LogInformation("Cliente removido: {CustomerId}", id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao remover cliente.");
            }
        }
    }
}
