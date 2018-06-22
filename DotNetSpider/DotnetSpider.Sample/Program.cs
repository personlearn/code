using DotnetSpider.Sample.docs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpider.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //#if NETCOREAPP
            //			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //#else
            //            ThreadPool.SetMinThreads(200, 200);
            //            OcrDemo.Process();
            //#endif
            //            AutoIncrementTargetUrlsExtractor.Run();

            //            MyTest();

            string ExcuteSpider = "AutoIncrementTargetUrlsExtractor";

            var type = Assembly.Load(MethodBase.GetCurrentMethod().DeclaringType.Namespace).GetTypes();
            foreach(var tp in type)
            {
                if(tp.Name == ExcuteSpider)
                {
                    tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
                }
            }
        }


        /// <summary>
        /// <c>MyTest</c> is a method in the <c>Program</c>
        /// </summary>
        private static void MyTest()
        {

        }
    }
}
