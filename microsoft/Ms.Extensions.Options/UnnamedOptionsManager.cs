﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;

namespace Ms.Extensions.Options
{
    internal sealed class UnnamedOptionsManager<TOptions> :
        IOptions<TOptions>
        where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private volatile object _syncObj;
        private volatile TOptions _value;

        public UnnamedOptionsManager(IOptionsFactory<TOptions> factory) => _factory = factory;

        public TOptions Value
        {
            get
            {
                if (_value is TOptions value)
                {
                    return value;
                }

                lock (_syncObj ?? Interlocked.CompareExchange(ref _syncObj, new object(), null) ?? _syncObj)
                {
                    if (_value == null)
                    {
                        _value = _factory.Create(Options.DefaultName);
                    }
                    return _value;
                }
            }
        }
    }
}
