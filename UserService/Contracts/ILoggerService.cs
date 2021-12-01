namespace UserService.Contracts
{
    public interface ILoggerService
    {
        /// <summary>Logs the information.</summary>
        /// <param name="message">The message to log.</param>
        void LogInfo(string message);

        /// <summary>Logs the information.</summary>
        /// <param name="message">The message to log.</param>
        void LogWarn(string message);

        /// <summary>Logs the warning.</summary>
        /// <param name="message">The warning message to log.</param>
        void LogDebug(string message);

        /// <summary>Logs the debug message.</summary>
        /// <param name="message">The debug message to log.</param>
        void LogError(string message);

    }
}
