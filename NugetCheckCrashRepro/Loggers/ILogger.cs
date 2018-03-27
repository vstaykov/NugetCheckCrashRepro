using System;

namespace NugetCheckCrashRepro.Loggers
{
    internal interface ILogger
    {
        void LogInfo(string message);

        void LogException(Exception exception);
    }
}
