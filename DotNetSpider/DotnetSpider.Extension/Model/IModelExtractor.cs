using DotnetSpider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Extension.Model
{
    public interface IModelExtractor
    {
        /// <summary>
        /// 解析成爬虫实体对象
        /// </summary>
        /// <param name="page">页面数据</param>
        /// <returns>爬虫实体对象</returns>
        IEnumerable<dynamic> Extract(Page page, IModel model);
    }
}
