using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationSource
    {
        IConfigurationProvider Build(IConfigurationBuilder builder);
    }
}
