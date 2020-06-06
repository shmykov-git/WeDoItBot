using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Tools
{
	public class DoubleFuncComparer<T> : FuncComparer<T, double>
	{
		public DoubleFuncComparer(Func<T, double> func, bool ascending = true) : base(func, ascending)
		{ }
	}
}