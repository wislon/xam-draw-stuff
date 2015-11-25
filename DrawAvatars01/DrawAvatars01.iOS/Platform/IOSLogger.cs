using System;
using System.Linq;
using Splat;

namespace DrawAvatars01.iOS.Platform
{
    public class 
        IOSLogger : ILogger
    {
        private const string TAG = "drawstuff";

        public void Write(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    {
                        LogMessage(LogLevel.Debug, message);
                        break;
                    }
                case LogLevel.Warn:
                    {   
                        LogMessage(LogLevel.Warn, message);
                        break;
                    }
                case LogLevel.Fatal: // exception thrown
                case LogLevel.Error:
                    {   
                        LogMessage(LogLevel.Error, message);
                        break;
                    }
                default:
                    {
                        LogMessage(LogLevel.Info, message);
                        break;
                    }
            }
        }

        private static void LogMessage(LogLevel loglevel, string message)
        {
            string formattedMessage = string.Format("{0:s} {1}/{2}  {3}",
                                                    DateTime.Now,
                                                    loglevel.ToString().First(),
                                                    TAG,
                                                    message);
#if (DEBUG)
            System.Diagnostics.Debug.WriteLine(formattedMessage);
#else
                  System.Console.WriteLine(formattedMessage);
#endif
        }

        public LogLevel Level { get; set; }
    }
}