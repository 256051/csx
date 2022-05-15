// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationSection : IConfiguration
    {
        string Value { get; set; }

        string Path { get; }

        string Key { get; }
    }
}
