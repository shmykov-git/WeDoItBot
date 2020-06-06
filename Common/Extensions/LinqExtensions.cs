using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
	public static class LinqExtensions
	{
		public static IEnumerable<int> ByIndex<T>(this IEnumerable<T> list)
		{
			var i = 0;
			foreach (var t in list)
			{
				yield return i++;
			}
		}

		public static IEnumerable<TRes> SelectByIndex<T, TRes>(this IEnumerable<T> list, Func<int, T, TRes> fn)
		{
			var i = 0;
			foreach (var t in list)
			{
				yield return fn(i++, t);
			}
		}

		public static IEnumerable<TRes> SelectPair<T, TRes>(this IEnumerable<T> list, Func<T, T, TRes> func)
		{
			var i = 0;
			var prevT = default(T);
			foreach (var t in list)
			{
				if (i++ % 2 == 1)
					yield return func(prevT, t);
				prevT = t;
			}
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (var t in list)
				action(t);
		}

		public static void ForEachPair<T>(this IEnumerable<T> list, Action<T, T> action)
		{
			list.SelectPair((a, b) =>
				{
					action(a, b);
					return true;
				})
				.ToArray();
		}

		public static void ForEachByIndex<T>(this IEnumerable<T> list, Action<int, T> action)
		{
			list.SelectByIndex((index, b) =>
				{
					action(index, b);
					return true;
				})
				.ToArray();
		}

		public static void ForBoth<T1, T2>(this (T1[], T2[]) lists, Action<T1, T2> action)
		{
			lists.SelectBoth((a, b) =>
				{
					action(a, b);
					return true;
				})
				.ToArray();
		}

		// TODO: performance
		public static void ForBothJoin<T, TKey>(this (T[], T[]) lists, Func<T, TKey> keyFn, Action<T, T> action) where TKey : IEquatable<TKey>
		{
			var A = lists.Item1;
			var B = lists.Item2;

			var keysQuery =
				from a in A
				join b in B on keyFn(a) equals keyFn(b)
				select keyFn(a);

			var keys = keysQuery.ToList();

			if (keys.Count == 0)
				return;

			var i = 0;
			var j = 0;
			while (i < A.Length && j < B.Length)
			{
				var hasA = keys.Contains(keyFn(A[i]));
				var hasB = keys.Contains(keyFn(B[j]));

				if (hasA && hasB)
				{
					action(A[i], B[j]);
					i++;
					j++;
				}
				else if (hasA)
				{
					j++;
				}
				else if (hasB)
				{
					i++;
				}
				else
				{
					i++;
					j++;
				}
			}
		}

		public static IEnumerable<TRes> SelectBoth<T1, T2, TRes>(this (T1[], T2[]) lists, Func<T1, T2, TRes> func)
		{
			return lists.Item1.ByIndex().Select(i => func(lists.Item1[i], lists.Item2[i]));
		}

		public static IEnumerable<TRes> SelectBoth<T1, T2, TRes>(this (List<T1>, List<T2>) lists, Func<T1, T2, TRes> func)
		{
			return lists.Item1.ByIndex().Select(i => func(lists.Item1[i], lists.Item2[i]));
		}
	}
}