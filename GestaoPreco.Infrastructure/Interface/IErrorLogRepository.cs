using GestaoPedido.Infrastructure.DTO;
using GestaoPedido.Infrastructure.Logging;

namespace Infrastructure
{
    public interface IErrorLogRepository
    {
        Task SaveAsync(ErrorLog log);

        Task<List<ErrorLog>> GetAllAsync();

        Task<List<ErrorLog>> SearchAsync(LogFilter filter);
    }
}