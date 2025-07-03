using Application;
using Application.Queries;
using Domain;
using DTO;
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPreco.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(IMediator mediator, IConfiguration configuration, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _mediator = mediator;
            _configuration = configuration;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _mediator.Send(new ListOrdersQuery());
                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar pedidos.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderByIdQuery { Id = id });

                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar pedido.");
            }
        }

        [HttpGet("with-items")]
        [ProducesResponseType(typeof(IEnumerable<OrderWithItemsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<OrderWithItemsDto>>> GetOrdersWithItems()
        {
            try
            {
                var orders = await _mediator.Send(new ListOrdersWithItemsQuery());

                var result = orders.Select(order => new OrderWithItemsDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                });

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar pedidos com itens.");
            }
        }

        [HttpGet("{id}/with-items")]
        [ProducesResponseType(typeof(OrderWithItemsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<OrderWithItemsDto>> GetOrderWithItemsById(Guid id)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderWithItemsByIdQuery(id));
                if (order == null)
                    return NotFound();

                var result = new OrderWithItemsDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao buscar pedido com itens.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fixedCustomerId = Guid.Parse(_configuration["OrderSettings:FixedCustomerId"]);
                command.CustomerId = fixedCustomerId;

                var orderId = await _mediator.Send(command);

                if (orderId == Guid.Empty)
                    return BadRequest();

                return Ok(orderId);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao criar pedido.");
            }
        }

        [HttpPost("create-with-items")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateWithItems([FromBody] CreateOrderWithItemsDto dto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { Errors = errors, ModelState = ModelState });
            }

            try
            {

                if (!Enum.IsDefined(typeof(OrderStatus), dto.Status))
                {
                    return BadRequest(new { Error = $"Status inválido: {dto.Status}. Valores válidos: {string.Join(", ", Enum.GetNames(typeof(OrderStatus)))}" });
                }

                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    OrderDate = dto.Date,
                    TotalAmount = dto.TotalAmount,
                    Status = dto.Status
                };

                await _orderRepository.AddAsync(order);

                foreach (var item in dto.Items)
                {


                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _orderItemRepository.AddAsync(orderItem);
                }

                return Ok(new { order.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Erro interno ao criar pedido com itens.", Details = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fixedCustomerId = Guid.Parse(_configuration["OrderSettings:FixedCustomerId"]);
                command.Id = id;
                command.CustomerId = fixedCustomerId;

                var success = await _mediator.Send(command);

                if (!success)
                    return NotFound();

                return Ok(true);
            }
            catch
            {
                return StatusCode(500, "Erro interno ao atualizar pedido.");
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
                var success = await _mediator.Send(new DeleteOrderCommand { Id = id });
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Erro interno ao deletar pedido.");
            }
        }
    }
}