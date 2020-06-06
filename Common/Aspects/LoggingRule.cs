using System;

namespace Common.Aspects
{
	[Flags]
	public enum LoggingRule
	{
		Ignore = 0,

		Args = 1,
		Result = 2,
		Exception = 4,

		NoCut = 8,
		Stabilize = 16,
		Performance = 32,

		Default = Args | Result,
		Input = Args | Exception,
		Output = Result | Exception,
		Full = Args | Result | Exception
	}
}