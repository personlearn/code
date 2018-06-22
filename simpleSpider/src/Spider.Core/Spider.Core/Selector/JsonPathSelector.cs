﻿using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spider.Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Selector
{
    /// <summary>
    /// JsonPath selector.
    /// Used to extract content from JSON.
    /// </summary>
    public class JsonPathSelector : ISelector
    {
        private readonly string _jsonPath;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="jsonPath">JsonPath</param>
        public JsonPathSelector(string jsonPath)
        {
            _jsonPath = jsonPath;
        }

        /// <summary>
        /// 从JSON文本中查询单个结果
        /// 如果符合条件的结果有多个, 仅返回第一个
        /// </summary>
        /// <param name="json">需要查询的Json文本</param>
        /// <returns>查询结果</returns>
        public dynamic Select(dynamic json)
        {
            IList<dynamic> result = SelectList(json);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }

        /// <summary>
        /// 从JSON文本中查询所有结果
        /// </summary>
        /// <param name="json">需要查询的Json文本</param>
        /// <returns>查询结果</returns>
        public IEnumerable<dynamic> SelectList(dynamic json)
        {
            if (json != null)
            {
                List<dynamic> list = new List<dynamic>();
                if (json is string)
                {
                    if (JsonConvert.DeserializeObject(json) is JObject)
                    {
                        JObject o = JsonConvert.DeserializeObject(json);
                        var items = o.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
                        if (items.Count > 0)
                        {
                            list.AddRange(items);
                        }
                    }
                    else
                    {
                        JArray array = JsonConvert.DeserializeObject(json) as JArray;
                        var items = array?.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
                        if (items != null && items.Count > 0)
                        {
                            list.AddRange(items);
                        }
                    }
                }
                else
                {
                    dynamic realText = json is HtmlNode ? JsonConvert.DeserializeObject<JObject>(json.InnerText) : json;

                    if (realText is JObject)
                    {
                        JObject o = realText;
                        var items = o.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
                        if (items.Count > 0)
                        {
                            list.AddRange(items);
                        }
                    }
                    else
                    {
                        JArray array = json as JArray;
                        var items = array?.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
                        if (items != null && items.Count > 0)
                        {
                            list.AddRange(items);
                        }
                    }
                }
                return list;
            }
            else
            {
                return Enumerable.Empty<dynamic>();
            }
        }
    }
}
