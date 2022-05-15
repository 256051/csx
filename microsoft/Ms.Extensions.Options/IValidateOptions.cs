// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Ms.Extensions.Options
{
    /// <summary>
    /// Interface used to validate options.
    /// </summary>
    /// <typeparam name="TOptions">The options type to validate.</typeparam>
    public interface IValidateOptions<TOptions> where TOptions : class
    {
        ValidateOptionsResult Validate(string name, TOptions options);
    }
}
