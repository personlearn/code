﻿using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Core.Redial;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpider.Core
{
    /// <summary>
	/// 通过Http向企业服务上报运行状态
	/// </summary>
	public class HttpExecuteRecord : IExecuteRecord
    {
        public ILogger Logger { get; set; }

        /// <summary>
        /// 添加运行记录
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <param name="name">任务名称</param>
        /// <param name="identity">任务标识</param>
        /// <returns>是否上报成功</returns>
        public bool Add(string taskId, string name, string identity)
        {
            if (string.IsNullOrWhiteSpace(taskId) || string.IsNullOrWhiteSpace(identity) || !Env.HubService)
            {
                return true;
            }

            try
            {
                var retryTimesPolicy = Policy.Handle<Exception>().Retry(10, (ex, count) =>
                {
                    Logger?.Error($"Try to add execute record failed [{count}]: {ex}", LogUtil.Identity);
                    Thread.Sleep(5000);
                });
                retryTimesPolicy.Execute(() =>
                {
                    NetworkCenter.Current.Execute("executeRecord", () =>
                    {
                        var response = HttpSender.Client.GetAsync($"{Env.HubServiceTaskApiUrl}/{taskId}?action=increase").Result;
                        response.EnsureSuccessStatusCode();
                    });
                });
                return true;
            }
            catch (Exception e)
            {
                Logger?.Error($"Add execute record failed: {e}", identity);
                return false;
            }
        }

        /// <summary>
        /// 删除运行记录
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <param name="name">任务名称</param>
        /// <param name="identity">任务标识</param>
        public void Remove(string taskId, string name, string identity)
        {
            if (string.IsNullOrWhiteSpace(taskId) || !Env.HubService)
            {
                return;
            }

            try
            {
                var retryTimesPolicy = Policy.Handle<Exception>().Retry(10, (ex, count) =>
                {
                    Logger?.Error($"Try to remove execute record failed [{count}]: {ex}", identity);
                    Thread.Sleep(5000);
                });
                retryTimesPolicy.Execute(() =>
                {
                    NetworkCenter.Current.Execute("executeRecord", () =>
                    {
                        var response = HttpSender.Client.GetAsync($"{Env.HubServiceTaskApiUrl}/{taskId}?action=reduce").Result;
                        response.EnsureSuccessStatusCode();
                    });
                });
            }
            catch (Exception e)
            {
                Logger?.Error($"Remove execute record failed: {e}", identity);
            }
        }
    }
}
