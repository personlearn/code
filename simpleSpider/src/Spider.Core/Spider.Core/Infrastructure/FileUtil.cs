using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Infrastructure
{
    /// <summary>
	/// 文件操作帮助类
	/// </summary>
	public static class FileUtil
    {
        /// <summary>
        /// 如果文件所在文件夹没有创建, 则帮助创建
        /// </summary>
        /// <param name="fullName">文件路径</param>
        /// <returns>文件对象</returns>
        public static FileInfo PrepareFile(string fullName)
        {
            DirectoryExtension.CheckAndMakeParentDirecotry(fullName);
            return new FileInfo(fullName);
        }
    }
}
