using Spider.Core.Downloader;
using Spider.Core.Monitor;
using Spider.Core.Pipeline;
using Spider.Core.Processor;
using Spider.Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    /// <summary>
	/// 爬虫接口定义
	/// </summary>
    public interface ISpider : IDisposable, IControllable, IAppBase
    {
        /// <summary>
        /// 采集站点的信息配置
        /// </summary>
        Site Site { get; }

        IScheduler Scheduler { get; }

        /// <summary>
        /// 下载器
        /// </summary>
        IDownloader Downloader { get; set; }

        /// <summary>
        /// 页面解析器
        /// </summary>
        IReadOnlyCollection<IPageProcessor> PageProcessors { get; }

        /// <summary>
        /// 数据管道
        /// </summary>
        IReadOnlyCollection<IPipeline> Pipelines { get; }

        /// <summary>
        /// 监控接口
        /// </summary>
        IMonitor Monitor { get; set; }
    }
}
