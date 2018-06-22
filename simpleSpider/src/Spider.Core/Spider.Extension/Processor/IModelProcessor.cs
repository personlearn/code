using Spider.Extension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Extension.Processor
{
    /// <summary>
	/// 针对爬虫模型的页面解析器、抽取器
	/// </summary>
	public interface IModelProcessor
    {
        /// <summary>
        /// 爬虫模型的定义
        /// </summary>
        IModel Model { get; }
    }
}
