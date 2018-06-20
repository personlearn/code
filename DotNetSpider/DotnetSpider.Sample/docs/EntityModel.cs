﻿using DotnetSpider.Core.Selector;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using DotnetSpider.Extension.Model.Formatter;
using DotnetSpider.Extension.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Sample.docs
{
    /// <summary>
	/// 如果想尝试把数据存到MySql中，请把 AddPipeline(new MySqlEntityPipeline("")); 中的连接字符串替换会您自己MySql的连接字符串
	/// 如果想尝试把数据存到SqlServer中，请使用 AddPipeline(new SqlServerEntityPipeline("")); 中的连接字符串替换会您自己SqlServer的连接字符串
	/// </summary>
	public class EntityModel
    {
        public static void Run()
        {
            Spider spider = new Spider();
            spider.Run();
        }

        private class Spider : EntitySpider
        {
            public Spider() : base("EntityModelSpider")
            {
            }

            protected override void MyInit(params string[] arguments)
            {
                var word = "可乐|雪碧";
                AddStartUrl(string.Format("http://news.baidu.com/ns?word={0}&tn=news&from=news&cl=2&pn=0&rn=20&ct=1", word), new Dictionary<string, dynamic> { { "Keyword", word } });
                AddEntityType<BaiduSearchEntry>();
                AddPipeline(new MySqlEntityPipeline("Database='mysql';Data Source=localhost;User ID=root;Port=3306;SslMode=None;"));
            }

            [TableInfo("baidu", "baidu_search_entity_model")]
            [EntitySelector(Expression = ".//div[@class='result']", Type = SelectorType.XPath)]
            class BaiduSearchEntry : BaseEntity
            {
                [Field(Expression = "Keyword", Type = SelectorType.Enviroment)]
                public string Keyword { get; set; }

                [Field(Expression = ".//h3[@class='c-title']/a")]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                public string Title { get; set; }

                [Field(Expression = ".//h3[@class='c-title']/a/@href")]
                public string Url { get; set; }

                [Field(Expression = ".//div/p[@class='c-author']/text()")]
                [ReplaceFormatter(NewValue = "-", OldValue = "&nbsp;")]
                public string Website { get; set; }

                [Field(Expression = ".//div/span/a[@class='c-cache']/@href")]
                public string Snapshot { get; set; }

                [Field(Expression = ".//div[@class='c-summary c-row ']", Option = FieldOptions.InnerText)]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
                public string Details { get; set; }

                [Field(Expression = ".", Option = FieldOptions.InnerText)]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
                public string PlainText { get; set; }
            }
        }
    }
}
