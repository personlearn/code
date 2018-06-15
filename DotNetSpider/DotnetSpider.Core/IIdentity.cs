using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core
{
    /// <summary>
	/// 唯一标识接口
	/// </summary>
	public interface IIdentity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        string Identity { get; set; }
    }
}
