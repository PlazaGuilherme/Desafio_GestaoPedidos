using MediatR;
using Domain;

namespace Application.Queries
{
    public class GetOrderWithItemsByIdQuery : IRequest<Order?>
    {
        public Guid OrderId { get; set; }

        public GetOrderWithItemsByIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}