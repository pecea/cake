using Common;
using Microsoft.CodeAnalysis;
using System;

namespace Build.Extensions
{
    internal static class DiagnosticSeverityExtensions
    {
        internal static LogLevel ToLogLevel(this DiagnosticSeverity severity)
        {
            switch (severity)
            {
                case DiagnosticSeverity.Hidden:
                    return LogLevel.Trace;
                case DiagnosticSeverity.Info:
                    return LogLevel.Debug;
                case DiagnosticSeverity.Warning:
                    return LogLevel.Warn;
                case DiagnosticSeverity.Error:
                    return LogLevel.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity));
            }
        }
    }
}
