﻿using DotnetSpider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotnetSpider.Core.Tests
{
    namespace DotnetSpider.Core.Test
    {
        public class TestSpider : Spider
        {
            public TestSpider()
            {
                Name = "hello";
            }
        }

        public class StartupTest
        {
            [Fact]
            public void AnalyzeUnCorrectArguments()
            {
                var args1 = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:", "abcd" };
                var arguments1 = Startup.Parse(args1).Arguments;
                Assert.Equal("", arguments1.First());

                var args2 = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a::::" };
                var arguments2 = Startup.Parse(args2).Arguments;
                Assert.Equal("", arguments2.First());

                var args3 = new[] { "-ti:DotnetSpider.Core.Tests.SpiderTests" };
                var arguments3 = Startup.Parse(args3);
                Assert.Null(arguments3);

                var args4 = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests" };
                var arguments4 = Startup.Parse(args4).Arguments;
                Assert.Empty(arguments4);
            }

            [Fact]
            public void AnalyzeArguments()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:a,b", "-n:myname" };
                var arguments = Startup.Parse(args);
                Assert.Equal("DotnetSpider.Core.Tests.SpiderTests", arguments.Spider);
                Assert.Equal("TestSpider", arguments.TaskId);
                Assert.Equal("guid", arguments.Identity);
                Assert.Equal("myname", arguments.Name);
                Assert.Equal("a", arguments.Arguments.ElementAt(0));
                Assert.Equal("b", arguments.Arguments.ElementAt(1));

                var args2 = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:    asdf" };
                var arguments2 = Startup.Parse(args2);
                Assert.Equal("asdf", arguments2.Arguments.ElementAt(0));

                var args3 = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid" };
                var arguments3 = Startup.Parse(args3);
                Assert.Equal("TestSpider", arguments3.TaskId);
            }

            [Fact]
            public void DetectCorrectSpiderCount()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                Assert.Single(spiderTypes);
            }

            [Fact]
            public void SetGuidIdentity()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                var spider = (TestSpider)Startup.CreateSpiderInstance("DotnetSpider.Core.Tests.SpiderTests", arguments, spiderTypes);
                Guid.Parse(spider.Identity);
            }

            [Fact]
            public void SetIdentity()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:WHAT", "-a:" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                var spider = (TestSpider)Startup.CreateSpiderInstance("DotnetSpider.Core.Tests.SpiderTests", arguments, spiderTypes);
                Assert.Equal("WHAT", spider.Identity);
            }

            [Fact]
            public void SetTaskId()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-i:guid", "-a:" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                var spider = (TestSpider)Startup.CreateSpiderInstance("DotnetSpider.Core.Tests.SpiderTests", arguments, spiderTypes);
                Assert.Equal("TestSpider", spider.TaskId);
            }

            [Fact]
            public void SetSpiderName()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-n:What", "-i:guid", "-a:" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                var spider = (TestSpider)Startup.CreateSpiderInstance("DotnetSpider.Core.Tests.SpiderTests", arguments, spiderTypes);
                Assert.Equal("What", spider.Name);
            }

            [Fact]
            public void SetReportArgument()
            {
                var args = new[] { "-s:DotnetSpider.Core.Tests.SpiderTests", "--tid:SpiderTests", "-n:What", "-i:guid", "-a:report" };
                var arguments = Startup.Parse(args);
                var spiderTypes = Startup.DetectSpiders();
                var spider = (TestSpider)Startup.CreateSpiderInstance("DotnetSpider.Core.Tests.SpiderTests", arguments, spiderTypes);
                Assert.Equal(1000, spider.EmptySleepTime);
            }
        }
    }
}