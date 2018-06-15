using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.HtmlAgilityPack
{
    internal static class ExtensionMethods
    {
        public static IEnumerable<TResult> SelectNonNull<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            foreach (var item in source)
            {
                var value = selector(item);
                if (value != null) yield return value;
            }
        }
    }
}
