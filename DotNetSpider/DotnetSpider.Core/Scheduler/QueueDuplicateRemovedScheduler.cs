﻿using DotnetSpider.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Core.Scheduler
{
    /// <summary>
	/// Basic Scheduler implementation.
	/// </summary>
	public class QueueDuplicateRemovedScheduler : DuplicateRemovedScheduler
    {
        private readonly object _lock = new object();
        private List<Request> _queue = new List<Request>();
        private readonly AutomicLong _successCounter = new AutomicLong(0);
        private readonly AutomicLong _errorCounter = new AutomicLong(0);

        public override bool IsDistributed => false;

        /// <summary>
        /// 是否会使用互联网
        /// </summary>
        protected override bool UseInternet { get; set; } = false;

        /// <summary>
        /// 如果链接不是重复的就添加到队列中
        /// </summary>
        /// <param name="request">请求对象</param>
        protected override void PushWhenNoDuplicate(Request request)
        {
            lock (_lock)
            {
                _queue.Add(request);
            }
        }

        /// <summary>
        /// Reset duplicate check.
        /// </summary>
        public override void ResetDuplicateCheck()
        {
            lock (_lock)
            {
                _queue.Clear();
            }
        }

        /// <summary>
        /// 取得一个需要处理的请求对象
        /// </summary>
        /// <returns>请求对象</returns>
        public override Request Poll()
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    return null;
                }
                else
                {
                    Request request;
                    switch (TraverseStrategy)
                    {
                        case TraverseStrategy.DFS:
                            {
                                request = _queue.Last();
                                _queue.RemoveAt(_queue.Count - 1);
                                break;
                            }
                        case TraverseStrategy.BFS:
                            {
                                request = _queue.First();
                                _queue.RemoveAt(0);
                                break;
                            }
                        default:
                            {
                                throw new NotImplementedException();
                            }
                    }

                    return request;
                }
            }
        }

        /// <summary>
        /// 剩余链接数
        /// </summary>
        public override long LeftRequestsCount
        {
            get
            {
                lock (_lock)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        /// 采集成功的链接数
        /// </summary>
        public override long SuccessRequestsCount => _successCounter.Value;

        /// <summary>
        /// 采集失败的次数, 不是链接数, 如果一个链接采集多次都失败会记录多次
        /// </summary>
        public override long ErrorRequestsCount => _errorCounter.Value;

        /// <summary>
        /// 采集成功的链接数加 1
        /// </summary>
        public override void IncreaseSuccessCount()
        {
            _successCounter.Inc();
        }

        /// <summary>
        /// 采集失败的次数加 1
        /// </summary>
        public override void IncreaseErrorCount()
        {
            _errorCounter.Inc();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="requests">请求对象</param>
        public override void Import(IEnumerable<Request> requests)
        {
            lock (_lock)
            {
                _queue = new List<Request>(requests);
            }
        }

        /// <summary>
        /// 取得队列中所有的请求对象
        /// </summary>
        public IReadOnlyCollection<Request> All
        {
            get
            {
                lock (_lock)
                {
                    return new ReadOnlyEnumerable<Request>(_queue.ToArray());
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            lock (_lock)
            {
                _successCounter.Set(0);
                _errorCounter.Set(0);
                _queue.Clear();
            }

            base.Dispose();
        }
    }
}
