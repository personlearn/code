using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Processor
{
    /// <summary>
	/// 页面解析器、抽取器
	/// </summary>
	public interface IPageProcessor
    {
        /// <summary>
        /// 解析数据结果, 解析目标链接
        /// </summary>
        /// <param name="page">页面数据</param>
        /// <param name="spider">爬虫对象</param>
        void Process(Page page, ISpider spider);
    }
}
