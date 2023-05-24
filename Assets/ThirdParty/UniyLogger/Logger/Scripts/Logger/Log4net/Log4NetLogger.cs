﻿using System;
using log4net;
using log4net.Core;

namespace Logger.Log4net
{
    internal class Log4NetLogger : ILogger
    {
        private readonly ILog _internalLogger;
        private readonly Type _type;

        public Log4NetLogger(ILog internalLogger, Type type)
        {
            _internalLogger = internalLogger;
            _type = type;
        }
        
        public void Trace(string message)
        {
            Write(Level.Trace, LogRecord.Create(message));
        }

        public void Trace(string message, Exception e)
        {
            Write(Level.Trace, LogRecord.Create(message).AddException(e));
        }
        
        public void Debug(string message)
        {
            Write(Level.Debug, LogRecord.Create(message));
        }

        public void Debug(string message, Exception e)
        {
            Write(Level.Debug, LogRecord.Create(message).AddException(e));
        }

        public void Info(string message)
        {
            Write(Level.Info, LogRecord.Create(message));
        }

        public void Info(string message, Exception e)
        {
            Write(Level.Info, LogRecord.Create(message).AddException(e));
        }
        
        
        public void Warn(string message)
        {
            Write(Level.Warn, LogRecord.Create(message));
        }

        public void Warn(string message, Exception e)
        {
            Write(Level.Warn, LogRecord.Create(message).AddException(e));
        }
        
        public void Error(string message)
        {
            Write(Level.Error, LogRecord.Create(message));
        }

        public void Error(string message, Exception e)
        {
            Write(Level.Error, LogRecord.Create(message).AddException(e));
        }

        public void Exception(Exception e)
        {
            Write(Level.Error, LogRecord.Create(null).AddException(e));
        }

        private void Write(Level logLevel, LogRecord logRecord)
        {
            _internalLogger.Logger.Log(_type, logLevel, logRecord.Message, logRecord.Exception);
        }
    }
}