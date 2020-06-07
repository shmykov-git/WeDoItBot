using System;
using System.Threading;

namespace Suit.Logs
{
    public class LogToDebug : ILog
    {
        public void Warn(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"WARN|{msg}");
        }

        public void Exception(Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception.ToString());
        }

        public void Info(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"INFO|{Thread.CurrentThread.ManagedThreadId}|{msg}");
        }

        public void Trace(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"TRACE|{DateTime.Now:HH:mm:ss.fff}|{msg}");
        }

        public void Debug(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG|{DateTime.Now:HH:mm:ss.fff}|{msg}");
        }

        public void Error(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR|{DateTime.Now:HH:mm:ss.fff}|{msg}");
        }

        public void Fatal(string msg)
        {
			System.Diagnostics.Debug.WriteLine($"FATAL|{DateTime.Now:HH:mm:ss.fff}|{msg}");
		}
    }
}