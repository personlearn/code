using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core.Infrastructure
{
    public class LogInfo
    {
        public string Identity { get; set; }
        public string NodeId { get; set; }
        public DateTimeOffset Logged { get; set; } = DateTime.UtcNow;
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
