using System.Collections.Generic;
using System.Linq;

namespace Suit.Extensions
{
    public static class IntExtensions
    {
        public static int ToGroupValue(this int[] arr, int num)
        {
            var i = 0;
            while (num >= arr[i % arr.Length])
            {
                num -= arr[i % arr.Length];
                i++;
            }

            return i;
        }
        public static string SJoin(this IEnumerable<int> list, string delimiter = ", ")
        {
            return string.Join(delimiter, list);
        }
    }
}