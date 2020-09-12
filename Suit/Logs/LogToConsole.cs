using System;

namespace Suit.Logs
{
    public class LogToConsole : ILog
    {
        public void Warn(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Exception(Exception exception)
        {
            Console.WriteLine(exception.ToString());
        }

        public void Info(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Trace(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Debug(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Error(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Fatal(string msg)
        {
            Console.WriteLine(msg);
		}
    }
}