using GestaoPedido.Infrastructure.DTO;
using GestaoPedido.Infrastructure.Logging;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Repository
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly AppDbContext _context;

        public ErrorLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ErrorLog log)
        {
            await _context.ErrorLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ErrorLog>> GetAllAsync()
        {
            return await _context.ErrorLogs
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ErrorLog>> SearchAsync(LogFilter filter)
        {
            IQueryable<ErrorLog> query = _context.ErrorLogs.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Message))
            {
                query = query.Where(x =>
                    x.Message.Contains(filter.Message));
            }

            if (filter.StatusCode.HasValue)
            {
                query = query.Where(x =>
                    x.StatusCode == filter.StatusCode.Value);
            }

            if (filter.InitialDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt >= filter.InitialDate.Value);
            }

            if (filter.FinalDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt <= filter.FinalDate.Value);
            }

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}