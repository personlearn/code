using Newtonsoft.Json;
using Spider.Core;
using Spider.Extension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Extension.Pipeline
{
    /// <summary>
    /// Print datas in console
    /// Usually used in test.
    /// </summary>
    public class ConsoleEntityPipeline : ModelPipeline
    {
        /// <summary>
        /// 打印爬虫实体解析器解析到的实体数据结果到控制台
        /// </summary>
        /// <param name="model">爬虫实体类的名称</param>
        /// <param name="datas">实体类数据</param>
        /// <param name="spider">爬虫</param>
        /// <returns>最终影响结果数量(如数据库影响行数)</returns>
        protected override int Process(IModel model, IEnumerable<dynamic> datas, ISpider spider)
        {
            foreach (var data in datas)
            {
                Console.WriteLine($"{model.Identity}: {JsonConvert.SerializeObject(data)}");
            }
            return datas.Count();
        }
    }
}
