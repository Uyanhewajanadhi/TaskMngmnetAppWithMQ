﻿using NLog;
using SharedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedModels.Services
{
    public class LoggerService : ILoggerService
    {
        public static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }


        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
