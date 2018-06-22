using System.Reflection;

namespace Spider.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string ExcuteSpider = "AutoIncrementTargetUrlsExtractor";

            var type = Assembly.Load(MethodBase.GetCurrentMethod().DeclaringType.Namespace).GetTypes();
            foreach (var tp in type)
            {
                if (tp.Name == ExcuteSpider)
                {
                    tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
                }
            }
        }
    }
}
