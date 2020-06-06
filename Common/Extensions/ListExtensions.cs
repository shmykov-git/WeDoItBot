using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
	public static class ListExtensions
	{
		public static List<int> ToIndexesSortedLike<T>(this List<T> aList, List<T> bList)
			where T : IComparable, IComparable<T>
		{
			var a = aList.SortIndexes();
			var bBack = bList.SortIndexes().ToBackIndexes();

			return bBack.Select(i => a[i]).ToList();
		}

		public static List<int> SortIndexes<T>(this List<T> list) where T : IComparable, IComparable<T>
		{
			Func<int, T> fn = i => list[i];
			var indexes = list.ByIndex().ToArray();
			Array.Sort(indexes, fn.ToComparer());

			return indexes.ToList();
		}

		public static List<int> ShiftIndexes(this List<int> list, int shift)
		{
			return list.Select(i => i + shift).ToList();
		}

		public static List<int> ToBackIndexes(this List<int> indexes)
		{
			var backIndexes = new int[indexes.Count];
			indexes.ForEach(i => backIndexes[indexes[i]] = i);

			return backIndexes.ToList();
		}
	}
}