using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Tools
{
	public class FuncComparer<T, TRes> : IComparer, IComparer<T> where TRes : IComparable, IComparable<TRes>
	{
		private readonly Func<T, TRes> func;
		private readonly bool ascending;

		public FuncComparer(Func<T, TRes> func, bool ascending = true)
		{
			this.func = func;
			this.ascending = ascending;
		}

		public int Compare(object x, object y)
		{
			return Compare((T)x, (T)y);
		}

		public int Compare(T x, T y)
		{
			return ascending ? func(x).CompareTo(func(y)) : func(y).CompareTo(func(x));
		}
	}
}