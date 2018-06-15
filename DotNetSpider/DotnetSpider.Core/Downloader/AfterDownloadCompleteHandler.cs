﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core.Downloader
{
    /// <summary>
	/// <see cref="IAfterDownloadCompleteHandler"/>
	/// </summary>
	public abstract class AfterDownloadCompleteHandler : Named, IAfterDownloadCompleteHandler
    {
        /// <summary>
        /// You can process page data, detect download status(whether is banned) and update Cookie here.
        /// </summary>
        /// <summary>
        /// 处理页面数据、检测下载情况(是否被反爬)、更新Cookie等操作
        /// </summary>
        /// <param name="page"><see cref="Page"/></param>
        /// <param name="downloader">下载器 <see cref="IDownloader"/></param>
        /// <param name="spider"><see cref="ISpider"/></param>
        public abstract void Handle(ref Page page, IDownloader downloader, ISpider spider);
    }
}
