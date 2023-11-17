namespace ItchioLibrary.Models
{
    public enum LogLevel
    {
        debug,
        info,
        warning,
        error
    }

    public class LogMessage
    {
        public LogLevel level;
        public string message;
    }
}
