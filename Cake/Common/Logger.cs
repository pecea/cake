namespace Common
{
    using System.Runtime.CompilerServices;

    using NLog;

    public static class Logger
    {
        public static void Info(string message, [CallerMemberName]string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);
            logger.Info(message);
        }

        public static void Warn(string message, [CallerMemberName] string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);
            logger.Warn(message); 
        }        
        
        public static void Error(string message, [CallerMemberName] string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);
            logger.Error(message); 
        }        
        
        public static void Fatal(string message, [CallerMemberName] string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);
            logger.Fatal(message); 
        }        
        
        public static void Debug(string message, [CallerMemberName] string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);
            logger.Debug(message); 
        }
    }
}
