﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core.Processor
{
    /// <summary>
	/// A simple PageProcessor.
	/// </summary>
	public class SimplePageProcessor : BasePageProcessor
    {
        /// <summary>
        /// 解析出页面的title和html
        /// </summary>
        /// <param name="page">页面数据</param>
        protected override void Handle(Page page)
        {
            page.AddResultItem("title", page.Selectable.XPath("//title"));
            page.AddResultItem("html", page.Content);
        }
    }
}
