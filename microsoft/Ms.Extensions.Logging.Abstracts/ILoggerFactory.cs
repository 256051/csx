// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Ms.Extensions.Logging.Abstracts
{
    /// <summary>
    /// Represents a type used to configure the logging system and create instances of <see cref="ILogger"/> from
    /// the registered <see cref="ILoggerProvider"/>s.
    /// </summary>
    public interface ILoggerFactory : IDisposable
    {
        ILogger CreateLogger(string categoryName);
    }
}
