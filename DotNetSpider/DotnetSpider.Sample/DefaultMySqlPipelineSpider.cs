using DotnetSpider.Core.Processor;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Sample
{
    public class DefaultMySqlPipelineSpider : CommonSpider
    {
        public DefaultMySqlPipelineSpider() : base("DefaultMySqlPipeline")
        {
        }

        protected override void MyInit(params string[] arguments)
        {
            var word = "可乐|雪碧";
            AddPipeline(new DefaultMySqlPipeline(Core.Env.DataConnectionString, "baidu", "mysql_baidu_search"));
            AddStartUrl(string.Format("http://news.baidu.com/ns?word={0}&tn=news&from=news&cl=2&pn=0&rn=20&ct=1", word), new Dictionary<string, dynamic> { { "Keyword", word } });

            var processor = new DefaultPageProcessor();
            processor.AddTargetUrlExtractor("//p[@id=\"page\"]", "&pn=[0-9]+&");
            AddPageProcessors(processor);
        }
    }
}
