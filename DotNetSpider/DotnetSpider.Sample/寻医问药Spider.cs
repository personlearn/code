﻿using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using DotnetSpider.Extension.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Sample
{
    public class 寻医问药Spider : EntitySpider
    {
        public 寻医问药Spider() : base("xywy", new Core.Site
        {
        })
        {
        }

        protected override void MyInit(params string[] arguments)
        {
            for (int i = 1; i <= 100; ++i)
            {
                AddStartUrl($"http://yao.xywy.com/search/?q=%E6%85%A2%E6%80%A7%E6%94%AF%E6%B0%94%E7%AE%A1%E7%82%8E&sort=complex&pricefilter=1&p={i}");
            }
            AddPipeline(new MySqlEntityFilePipeline(MySqlEntityFilePipeline.FileType.InsertSql));
            AddEntityType<Item>();
        }

        [TableInfo("test", "yaoping")]
        [EntitySelector(Expression = ".//div[@class='s-list-btn']")]
        class Item
        {
            [Field(Expression = "./div[1]/div[1]/span[1]", Length = 100, Option = FieldOptions.InnerText)]
            public string name { get; set; }

            [Field(Expression = "./div[1]/div[1]/span[2]", Length = 100, Option = FieldOptions.InnerText)]
            public string corp { get; set; }
        }
    }
}
