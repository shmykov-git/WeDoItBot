using System;

namespace Suit.Logs
{
    public interface ILog
    {
        void Warn(string msg);

        void Exception(Exception e);

        void Info(string msg);

        void Debug(string msg);

        void Trace(string msg);

        void Error(string msg);

        void Fatal(string msg);
    }
}