using System;

namespace Common.Tools
{
	public static class Comparers
	{
		public static DoubleFuncComparer<int> IndexFunc(Func<int, double> func, bool ascending = true)
		{
			return new DoubleFuncComparer<int>(func, ascending);
		}
	}
}