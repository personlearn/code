using DotnetSpider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Sample
{
    [TaskName("CustomSpider1")]
    public class CustomSpider1 : AppBase
    {
        protected override void Execute(params string[] arguments)
        {
            Console.WriteLine("hello");
        }
    }
}
