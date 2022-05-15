// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Ms.Extensions.Logging.Abstracts;
using System.Collections.Generic;

namespace Ms.Extensions.Logging
{
    /// <summary>
    /// The options for a LoggerFilter.
    /// </summary>
    public class LoggerFilterOptions
    {
        public LogLevel MinLevel { get; set; }

        public bool CaptureScopes { get; set; } = true;

        internal List<LoggerFilterRule> RulesInternal { get; } = new List<LoggerFilterRule>();
    }
}
