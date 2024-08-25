using System;

namespace com.achieve.scripting.purchase
{
    public static class PurchaseLog
    {
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            None
        }

        /// <summary>
        /// 현재 로깅 레벨 설정
        /// </summary>
        public static LogLevel CurrentLogLevel { get; set; } = LogLevel.Debug;

        public static void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public static void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public static void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        private static void Log(LogLevel level, string message)
        {
            if (level >= CurrentLogLevel)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        UnityEngine.Debug.Log($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
                        break;
                    case LogLevel.Info:
                        UnityEngine.Debug.Log($"<color=green>[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}</color>");
                        break;
                    case LogLevel.Warning:
                        UnityEngine.Debug.Log($"<color=yellow>[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}</color>");
                        break;
                    case LogLevel.Error:
                        UnityEngine.Debug.Log($"<color=red>[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}</color>");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}