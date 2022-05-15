using System;

namespace Ms.Extensions.FileProviders
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    [Flags]
    public enum ExclusionFilters
    {
        Sensitive = DotPrefixed | Hidden | System,

        DotPrefixed = 1,

        Hidden = 2,

        System = 4,

        None = 0
    }
}
