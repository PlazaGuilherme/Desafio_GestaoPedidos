using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.DTO
{
    public class LogFilter
    {
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
    }
}
