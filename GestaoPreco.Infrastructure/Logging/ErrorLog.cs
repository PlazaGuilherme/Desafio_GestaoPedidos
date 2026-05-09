using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.Logging
{
    public class ErrorLog
    {
        public Guid Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public string StackTrace { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Method { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
