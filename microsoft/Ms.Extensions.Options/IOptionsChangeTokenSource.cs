

using Microsoft.Extensions.Primitives;

namespace Ms.Extensions.Options
{
    public interface IOptionsChangeTokenSource<out TOptions>
    {
        /// <summary>
        /// Returns a <see cref="IChangeToken"/> which can be used to register a change notification callback.
        /// </summary>
        /// <returns>Change token.</returns>
        IChangeToken GetChangeToken();

        /// <summary>
        /// The name of the option instance being changed.
        /// </summary>
        string Name { get; }
    }
}
