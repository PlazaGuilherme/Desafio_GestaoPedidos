using Domain;
using DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerDto[]), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
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
                var customer = await _customerRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customer = new Customer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                await _customerRepository.AddAsync(customer);
                _logger.LogInformation("Cliente criado: {CustomerId}", customer.Id);

                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, CustomerDto.FromEntity(customer));
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                    return NotFound();

                existingCustomer.Name = dto.Name;
                existingCustomer.Email = dto.Email;
                existingCustomer.Phone = dto.Phone;

                await _customerRepository.UpdateAsync(existingCustomer);
                _logger.LogInformation("Cliente atualizado: {CustomerId}", id);

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao atualizar cliente.");
            }
        }
    }
}
