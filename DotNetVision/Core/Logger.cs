using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DotNetVision.Core
{
    /// <summary>
    /// Centralized logging system for DotNetVision library
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private static readonly object _lock = new object();
        
        private bool _enabled = false;
        private string? _logFilePath;
        private readonly object _fileLock = new object();

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new Logger();
                    }
                }
                return _instance;
            }
        }

        private Logger() { }

        /// <summary>
        /// Enable logging to the specified file
        /// </summary>
        public void EnableLogging(string logFilePath)
        {
            lock (_fileLock)
            {
                _logFilePath = logFilePath;
                _enabled = true;
                
                // Create or clear the log file
                try
                {
                    File.WriteAllText(_logFilePath, $"DotNetVision Log Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\n");
                }
                catch
                {
                    // If we can't write to file, disable logging
                    _enabled = false;
                }
            }
        }

        /// <summary>
        /// Disable logging
        /// </summary>
        public void DisableLogging()
        {
            lock (_fileLock)
            {
                _enabled = false;
                _logFilePath = null;
            }
        }

        /// <summary>
        /// Log a message with timestamp and thread info
        /// </summary>
        public void Log(string component, string method, string message)
        {
            if (!_enabled || string.IsNullOrEmpty(_logFilePath)) return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var logEntry = $"[{timestamp}] [T{threadId}] [{component}.{method}] {message}\n";

            lock (_fileLock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logEntry);
                }
                catch
                {
                    // Silently fail if we can't write to log
                }
            }
        }

        /// <summary>
        /// Log method entry with parameters
        /// </summary>
        public void LogEntry(string component, string method, string? parameters = null)
        {
            var message = "ENTER";
            if (!string.IsNullOrEmpty(parameters))
                message += $" - Params: {parameters}";
            Log(component, method, message);
        }

        /// <summary>
        /// Log method exit
        /// </summary>
        public void LogExit(string component, string method, string? result = null)
        {
            var message = "EXIT";
            if (!string.IsNullOrEmpty(result))
                message += $" - Result: {result}";
            Log(component, method, message);
        }

        /// <summary>
        /// Log an error
        /// </summary>
        public void LogError(string component, string method, Exception ex)
        {
            Log(component, method, $"ERROR: {ex.Message} - StackTrace: {ex.StackTrace}");
        }

        /// <summary>
        /// Log an error with custom message
        /// </summary>
        public void LogError(string component, string method, string error)
        {
            Log(component, method, $"ERROR: {error}");
        }

        /// <summary>
        /// Log performance timing
        /// </summary>
        public void LogTiming(string component, string method, TimeSpan elapsed, string? details = null)
        {
            var message = $"TIMING: {elapsed.TotalMilliseconds:F2}ms";
            if (!string.IsNullOrEmpty(details))
                message += $" - {details}";
            Log(component, method, message);
        }
    }

    /// <summary>
    /// Helper class for timing method execution
    /// </summary>
    public class LogTimer : IDisposable
    {
        private readonly string _component;
        private readonly string _method;
        private readonly DateTime _startTime;
        private readonly string? _details;

        public LogTimer(string component, string method, string? details = null)
        {
            _component = component;
            _method = method;
            _details = details;
            _startTime = DateTime.Now;
        }

        public void Dispose()
        {
            var elapsed = DateTime.Now - _startTime;
            Logger.Instance.LogTiming(_component, _method, elapsed, _details);
        }
    }
}
