﻿using DotnetSpider.Core;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotnetSpider.Core.Tests
{
    internal class CountResult
    {
        public int Count { get; set; }
    }


    public partial class SpiderTest
    {
        [Fact(DisplayName = "RunAsyncAndStop")]
        public void RunAsyncAndStop()
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") == "1")
            {
                return;
            }
            Spider spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ThreadNum = 1;
            for (int i = 0; i < 10000; i++)
            {
                spider.AddStartUrl("http://www.baidu.com/" + i);
            }
            spider.RunAsync();
            Thread.Sleep(5000);
            spider.Pause(() =>
            {
                spider.RunAsync();
            });
            Thread.Sleep(3000);
        }

        [Fact(DisplayName = "RunAsyncAndContiune")]
        public void RunAsyncAndContiune()
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") == "1")
            {
                return;
            }
            Spider spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ThreadNum = 1;
            for (int i = 0; i < 10000; i++)
            {
                spider.AddStartUrl("http://www.baidu.com/" + i);
            }
            spider.RunAsync();
            Thread.Sleep(5000);
            spider.Pause(() =>
            {
                spider.Contiune();
            });
            Thread.Sleep(5000);
        }

        [Fact(DisplayName = "RunAsyncAndStopThenExit")]
        public void RunAsyncAndStopThenExit()
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") == "1")
            {
                return;
            }
            Spider spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ThreadNum = 1;
            for (int i = 0; i < 10000; i++)
            {
                spider.AddStartUrl("http://www.baidu.com/" + i);
            }
            spider.RunAsync();
            Thread.Sleep(5000);
            spider.Pause(() =>
            {
                spider.Exit();
            });
            Thread.Sleep(5000);
        }

        [Fact(DisplayName = "NoPipeline")]
        public void NoPipeline()
        {
            Spider spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor());
            spider.EmptySleepTime = 1000;
            spider.Run();
        }

        [Fact(DisplayName = "WhenNoStartUrl")]
        public void WhenNoStartUrl()
        {
            Spider spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ThreadNum = 1;
            spider.Run();

            Assert.Equal(Status.Finished, spider.Status);
        }

        internal class TestPipeline : BasePipeline
        {
            public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
            {
                foreach (var resultItem in resultItems)
                {
                    foreach (var entry in resultItem.Results)
                    {
                        Console.WriteLine($"{entry.Key}:{entry.Value}");
                    }
                }
            }
        }

        internal class TestPageProcessor : BasePageProcessor
        {
            protected override void Handle(Page page)
            {
                page.Skip = true;
            }
        }

        [Fact(DisplayName = "RetryWhenResultIsEmpty")]
        public void RetryWhenResultIsEmpty()
        {
            Spider spider = Spider.Create(new Site { CycleRetryTimes = 5, EncodingName = "UTF-8", SleepTime = 1000 }, new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ThreadNum = 1;
            spider.AddStartUrl("http://taobao.com");
            spider.Run();

            Assert.Equal(Status.Finished, spider.Status);
        }

        [Fact(DisplayName = "CloseSignal")]
        public void CloseSignal()
        {
            Spider spider = Spider.Create(new Site { CycleRetryTimes = 5, EncodingName = "UTF-8" },
                new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider.ClearSchedulerAfterCompleted = false;
            for (int i = 0; i < 20; ++i)
            {
                spider.AddStartUrl($"http://www.baidu.com/_t={i}");
            }
            var task = spider.RunAsync();
            Thread.Sleep(500);
            spider.SendExitSignal();
            task.Wait();
            Assert.Equal(10, spider.Scheduler.SuccessRequestsCount);

            Spider spider2 = Spider.Create(new Site { CycleRetryTimes = 5, EncodingName = "UTF-8" },
                new TestPageProcessor()).AddPipeline(new TestPipeline());
            spider2.ClearSchedulerAfterCompleted = false;
            for (int i = 0; i < 25; ++i)
            {
                spider2.AddStartUrl($"http://www.baidu.com/_t={i}");
            }
            spider2.Run();
            Assert.Equal(25, spider2.Scheduler.SuccessRequestsCount);
        }

        [Fact(DisplayName = "FastExit")]
        public void FastExit()
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") == "1")
            {
                return;
            }
            var path = "FastExit_Result.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Spider spider = Spider.Create(new Site { CycleRetryTimes = 5, EncodingName = "UTF-8", SleepTime = 0 },
                new FastExitPageProcessor())
                .AddPipeline(new FastExitPipeline());
            spider.ThreadNum = 1;
            spider.EmptySleepTime = 0;
            spider.AddStartUrl("http://item.jd.com/1013286.html?_t=1");
            spider.AddStartUrl("http://item.jd.com/1013286.html?_t=2");
            spider.AddStartUrl("http://item.jd.com/1013286.html?_t=3");
            spider.AddStartUrl("http://item.jd.com/1013286.html?_t=4");
            spider.AddStartUrl("http://item.jd.com/1013286.html?_t=5");
            spider.Run();
            stopwatch.Stop();
            var costTime = stopwatch.ElapsedMilliseconds;
            Assert.True(costTime < 3000);
            var results = File.ReadAllLines("FastExit_Result.txt");
            Assert.Contains("http://item.jd.com/1013286.html?_t=1", results);
            Assert.Contains("http://item.jd.com/1013286.html?_t=2", results);
            Assert.Contains("http://item.jd.com/1013286.html?_t=3", results);
            Assert.Contains("http://item.jd.com/1013286.html?_t=4", results);
            Assert.Contains("http://item.jd.com/1013286.html?_t=5", results);
        }

        internal class FastExitPageProcessor : BasePageProcessor
        {
            protected override void Handle(Page page)
            {
                page.AddResultItem("a", "b");
            }
        }

        internal class FileDownloader : BaseDownloader
        {
            protected override Task<Page> DowloadContent(Request request, ISpider spider)
            {
                return Task.FromResult(new Page(request));
            }
        }

        internal class FastExitPipeline : BasePipeline
        {
            public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
            {
                File.AppendAllLines("FastExit_Result.txt", new[] { resultItems.First().Request.Url.ToString() });
            }
        }
        //[Fact]
        //public void TestReturnHttpProxy()
        //{
        //	Spider spider = Spider.Create(new Site { HttpProxyPool = new HttpProxyPool(new KuaidailiProxySupplier("代理链接")), EncodingName = "UTF-8", MinSleepTime = 1000, Timeout = 20000 }, new TestPageProcessor()).AddPipeline(new TestPipeline()).SetThreadNum(1);
        //	for (int i = 0; i < 500; i++)
        //	{
        //		spider.AddStartUrl("http://www.taobao.com/" + i);
        //	}
        //	spider.Run();

        //	Assert.Equal(Status.Finished, spider.StatusCode);
        //}
    }
}