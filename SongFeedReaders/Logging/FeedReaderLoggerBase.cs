﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace SongFeedReaders.Logging
{
    public abstract class FeedReaderLoggerBase
    {
        private string? _loggerName;
        private LogLevel? _logLevel;
        private bool? _shortSource;
        private bool? _enableTimeStamp;

        public string? LoggerName
        {
            get { return _loggerName ?? LogController?.LoggerName; }
            set { _loggerName = value; }
        }
        public LogLevel LogLevel
        {
            get { return _logLevel ?? LogController?.LogLevel ?? LogLevel.Disabled; }
            set { _logLevel = value; }
        }
        public bool ShortSource
        {
            get { return _shortSource ?? LogController?.ShortSource ?? false; }
            set { _shortSource = value; }
        }
        public bool EnableTimestamp
        {
            get { return _enableTimeStamp ?? LogController?.EnableTimestamp ?? true; }
            set { _enableTimeStamp = value; }
        }
        private LoggingController? _loggingController;
        public LoggingController LogController
        {
            get { return _loggingController ?? LoggingController.DefaultLogController; }
            set
            {
                if (_loggingController == value)
                    return;
                _loggingController = value;
            }
        }

        public abstract void Log(string message, LogLevel logLevel,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0);

        public abstract void Log(string message, Exception e, LogLevel logLevel,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0);

        public void Trace(string message,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, LogLevel.Trace, file, member, line);
        public void Debug(string message,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, LogLevel.Debug, file, member, line);
        public void Info(string message,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, LogLevel.Info, file, member, line);
        public void Warning(string message,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, LogLevel.Warning, file, member, line);
#pragma warning disable CA1716 // Identifiers should not match keywords
        public void Error(string message,
#pragma warning restore CA1716 // Identifiers should not match keywords
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, LogLevel.Error, file, member, line);
        public void Exception(string message, Exception e,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0) => Log(message, e, LogLevel.Exception, file, member, line);

    }
    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Exception = 5,
        Disabled = 6
    }
}
