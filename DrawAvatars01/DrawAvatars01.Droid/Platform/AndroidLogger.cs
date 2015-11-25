using Android.Util;
using Splat;

namespace DrawAvatars01.Droid.Platform
{
    public class AndroidLogger : ILogger
    {
        private const string TAG = "drawstuff";

        public void Write(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                {
                        LogMessage(LogLevel.Debug, message);
                        Log.Debug(TAG, message);
                        break;
                    }
                case LogLevel.Warn:
                {   // TODO track warnings in Xamarin Insights
                    LogMessage(LogLevel.Warn, message);
                        Log.Warn(TAG, message);
                        break;
                    }
                case LogLevel.Fatal: // exception thrown
                case LogLevel.Error:
                {   // TODO track errors and exceptions in Xamarin Insights
                    LogMessage(LogLevel.Error, message);
                        Log.Error(TAG, message);
                        break;
                    }
                default:
                    {
                        LogMessage(LogLevel.Info, message);
                        Log.Info(TAG, message);
                        break;
                    }
            }
        }

        private static void LogMessage(LogLevel loglevel, string message)
        {
#if !DEBUG
            string formattedMessage = string.Format("{0:s} {1}/{2}  {3}",
                                                    DateTime.Now,
                                                    loglevel.ToString().First(),
                                                    TAG,
                                                    message);

            Console.WriteLine(formattedMessage);
#endif
        }

        public LogLevel Level { get; set; }
    }
}