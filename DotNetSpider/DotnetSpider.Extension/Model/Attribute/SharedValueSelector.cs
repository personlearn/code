﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Extension.Model.Attribute
{
    /// <summary>
	/// 共享值的查询器定义. 只能申明在一个爬虫实体类上面. 此查询器的结果会添加到Enviroment中, 即在爬虫实体类中的属性定义EnviromentSelector可以获取到共享值.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SharedValueSelector : Selector
    {
        /// <summary>
        /// 共享值的名称
        /// </summary>
        public string Name { get; set; }
    }
}
