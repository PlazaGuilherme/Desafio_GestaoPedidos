using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItem>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _context.OrderItens.ToListAsync();
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
                var item = await _context.OrderItens.FindAsync(id);
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
        public async Task<IActionResult> Create([FromBody] OrderItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.OrderItens.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.OrderItens.FindAsync(id);
                if (existing == null)
                    return NotFound();

                existing.ProductId = item.ProductId;
                existing.ProductName = item.ProductName;
                existing.Quantity = item.Quantity;
                existing.UnitPrice = item.UnitPrice;
                existing.TotalPrice = item.TotalPrice;
                existing.OrderId = item.OrderId;

                await _context.SaveChangesAsync();
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
                var existing = await _context.OrderItens.FindAsync(id);
                if (existing == null)
                    return NotFound();

                _context.OrderItens.Remove(existing);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao deletar item do pedido.");
            }
        }
    }
}