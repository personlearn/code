﻿using Spider.Core.Infrastructure;
using Spider.Core.Proxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Site
    {
        private Encoding _encoding = Encoding.UTF8;
        private string _encodingName;
        private ConcurrentBag<Request> _startRequests = new ConcurrentBag<Request>();

        /// <summary>
        /// 代理池
        /// </summary>
        public IHttpProxyPool HttpProxyPool { get; set; }

        /// <summary>
        /// 是否去除外链
        /// </summary>
        public bool RemoveOutboundLinks { get; set; }

        /// <summary>
        /// 采集目标的Domain, 如果RemoveOutboundLinks为True, 则Domain不同的链接会被排除, 如果RemoveOutboundLinks为False, 此值没有作用
        /// 需要自行设置
        /// </summary>
        public string[] Domains { get; set; }

        /// <summary>
        /// 配置下载器下载的内容是Json还是Html, 如果是Auto则会自动检测下载的内容, 建议设为Auto
        /// </summary>
        public ContentType ContentType { get; set; } = ContentType.Auto;

        /// <summary>
        /// 设置是否下载文件、图片
        /// </summary>
        public bool DownloadFiles { get; set; }

        /// <summary>
        /// 使用何种编码读取下载的内容, 如果没有设置编码, 下载器会尝试自动识别编码。
        /// 通过设置EncodingName才能修改此值
        /// </summary>
        public Encoding Encoding => _encoding;

        /// <summary>
        /// 目标链接的最大重试次数
        /// </summary>
        public int CycleRetryTimes { get; set; } = 5;

        /// <summary>
        /// 设置 User Agent 头
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";

        /// <summary>
        /// 设置 Accept 头
        /// </summary>
        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

        /// <summary>
        /// 设置全局的HTTP头, 下载器下载数据时会带上所有的HTTP头
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 每处理完一个目标链接后停顿的时间, 单位毫秒 
        /// </summary>
        public int SleepTime { get; set; } = 100;

        /// <summary>
        /// 去除返回的JSON数据的最外层填补
        /// </summary>
        public string JsonPadding { get; set; }

        /// <summary>
        /// 起始请求
        /// </summary>
        public IReadOnlyCollection<Request> StartRequests => new ReadOnlyEnumerable<Request>(_startRequests);

        /// <summary>
        /// 设置站点的编码 
        /// 如果没有设值, 下载器会尝试自动识别编码
        /// </summary>
        public string EncodingName
        {
            get { return _encodingName; }
            set
            {
                if (_encodingName != value)
                {
                    _encodingName = value;
                    _encoding = string.IsNullOrEmpty(_encodingName) ? null : Encoding.GetEncoding(_encodingName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        internal Page AddToCycleRetry(Request request)
        {
            Page page = new Page(request)
            {
                ContentType = ContentType
            };

            request.CycleTriedTimes++;

            if (request.CycleTriedTimes <= CycleRetryTimes)
            {
                request.Priority = 0;
                page.AddTargetRequest(request, false);
                page.Retry = true;
                return page;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加一个请求对象到当前站点
        /// </summary>
        /// <param name="request">请求对象</param>
        public void AddStartRequest(Request request)
        {
            AddStartRequests(request);
        }

        /// <summary>
        /// 添加一个请求对象到当前站点
        /// </summary>
        /// <param name="request">请求对象</param>
        public void AddStartRequests(params Request[] requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            AddStartRequests(requests.AsEnumerable());
        }

        /// <summary>
        /// 添加请求对象到当前站点
        /// </summary>
        /// <param name="requests">请求对象</param>
        public void AddStartRequests(IEnumerable<Request> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            foreach (var request in requests)
            {
                _startRequests.Add(request);
            }
        }

        /// <summary>
        /// 清空所有起始链接
        /// </summary>
        public void ClearStartRequests()
        {
            _startRequests = new ConcurrentBag<Request>();
        }

        /// <summary>
        /// 添加多个起始链接到当前站点 
        /// </summary>
        /// <param name="urls">起始链接</param>
        public void AddStartUrls(IEnumerable<string> urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException($"{nameof(urls)} should not be null.");
            }
            foreach (var url in urls)
            {
                AddStartRequest(new Request(url, null));
            }
        }
    }
}
