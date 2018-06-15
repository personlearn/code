﻿using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core
{
    /// <summary>
	/// 默认爬虫, 用于测试和一些默认情况使用, 框架使用者可忽略
	/// </summary>
	public class DefaultSpider : Spider
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public DefaultSpider() : this(Guid.NewGuid().ToString("N"), new Site())
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">爬虫标识</param>
        /// <param name="site">网站信息</param>
        public DefaultSpider(string id, Site site) : base(site, id, new QueueDuplicateRemovedScheduler(), new[] { new SimplePageProcessor() }, new[] { new ConsolePipeline() })
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">爬虫标识</param>
        /// <param name="site">网站信息</param>
        /// <param name="scheduler">URL队列</param>
        public DefaultSpider(string id, Site site, IScheduler scheduler) : base(site, id, scheduler, new[] { new SimplePageProcessor() }, new[] { new ConsolePipeline() })
        {
        }
    }
}
