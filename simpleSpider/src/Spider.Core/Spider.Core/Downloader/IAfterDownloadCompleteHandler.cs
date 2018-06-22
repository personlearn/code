﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Downloader
{
    /// <summary>
	/// Post-process after downloading.
	/// </summary>
	/// <summary xml:lang="zh-CN">
	/// 下载器完成下载工作后运行的处理工作
	/// </summary>
	public interface IAfterDownloadCompleteHandler
    {
        /// <summary>
        /// You can process page content, check downdload status(anti-spider) and update cookies etc. here.
        /// </summary>
        /// <summary xml:lang="zh-CN">
        /// 处理页面数据、检测下载情况(是否被反爬)、更新Cookie等操作
        /// </summary>
        /// <param name="page">页面数据 <see cref="Page"/></param>
        /// <param name="downloader">下载器 <see cref="IDownloader"/></param>
        /// <param name="spider">爬虫 <see cref="ISpider"/></param>
        void Handle(ref Page page, IDownloader downloader, ISpider spider);
    }
}
