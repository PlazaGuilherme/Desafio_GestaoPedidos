using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPedido.Domain.Classes
{
    public class ErrorLog
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Path { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
