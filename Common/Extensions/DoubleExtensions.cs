using System;
using System.Linq;

namespace Common.Extensions
{
	public static class DoubleExtensions
	{
		public static bool IsZero(this double value, double epsilon = double.Epsilon)
		{
			return Math.Abs(value) <= epsilon;
		}

		public static bool IsNotZero(this double value, double epsilon = double.Epsilon)
		{
			return !value.IsZero(epsilon);
		}

		public static int Exp(this double value)
		{
			var exp = int.Parse(value.ToString("E2").Split('E')[1]);

			return exp;
		}
	}
}