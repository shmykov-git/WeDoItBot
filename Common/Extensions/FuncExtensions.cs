using System;
using Common.Tools;

namespace Common.Extensions
{
	public static class FuncExtensions
	{
		public static FuncComparer<T, TRes> ToComparer<T, TRes>(this Func<T, TRes> func)
			where TRes : IComparable, IComparable<TRes>
		{
			return new FuncComparer<T, TRes>(func);
		}
	}
}