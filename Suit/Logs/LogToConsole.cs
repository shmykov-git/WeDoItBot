using System;
using System.Threading;

namespace Suit.Logs
{
    public class LogToConsole : ILog
    {
        private void Log(string level, string msg) =>
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}|{Thread.CurrentThread.ManagedThreadId}|{level}| {msg}");

        public void Warn(string msg)
        {
            Log("WARN ", msg);
        }

        public void Exception(Exception exception)
        {
            Log("EXCEP", exception.ToString());
        }

        public void Info(string msg)
        {
            Log("INFO ", msg);
        }

        public void Trace(string msg)
        {
            Log("TRACE", msg);
        }

        public void Debug(string msg)
        {
            Log("DEBUG", msg);
        }

        public void Error(string msg)
        {
            Log("ERROR", msg);
        }

        public void Fatal(string msg)
        {
            Log("FATAL", msg);
		}
    }
}