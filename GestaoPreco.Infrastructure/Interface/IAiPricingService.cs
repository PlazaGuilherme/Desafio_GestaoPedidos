using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.Interface
{
    public interface IAiPricingService
    {
        Task<string> AnalyzePrice(string product, decimal currentPrice);
    }
}
