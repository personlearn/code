using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Extension.Downloader
{
    /// <summary>
	/// WebDriver的操作, 如滚动、点击
	/// </summary>
	public interface IWebDriverHandler
    {
        /// <summary>
        /// 实现操作 WebDriver
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <returns>是否操作成功</returns>
        bool Handle(RemoteWebDriver driver);
    }
}
