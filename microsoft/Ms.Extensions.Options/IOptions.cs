// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Ms.Extensions.Options
{
    /// <summary>
    /// Used to retrieve configured <typeparamref name="TOptions"/> instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptions<out TOptions>
        where TOptions : class
    {
        TOptions Value { get; }
    }
}
