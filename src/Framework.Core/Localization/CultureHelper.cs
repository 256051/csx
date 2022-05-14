using System;
using System.Globalization;

namespace Framework.Core.Localization
{
    public static class CultureHelper
    {
        /// <summary>
        /// 切换语言临时
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="uiCulture"></param>
        /// <returns></returns>
        public static IDisposable Use(CultureInfo culture, CultureInfo uiCulture = null)
        {
            Check.NotNull(culture, nameof(culture));
            var currentCulture = CultureInfo.CurrentCulture;
            var currentUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture ?? culture;
            return new DisposeAction(() =>
            {
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentUiCulture;
            });
        }
    }
}
