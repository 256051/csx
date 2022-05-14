using System.Threading.Tasks;

namespace Framework.Word.Npoi
{
    public interface IWordConverter
    {
        /// <summary>
        /// word转html
        /// </summary>
        /// <param name="wordBytes">word文件 字节</param>
        /// <param name="htmlPageTitle">html页面title</param>
        Task<byte[]> ConvertToHtmlAsync(byte[] byteArray, string htmlPageTitle = default);

        /// <summary>
        /// word转html
        /// </summary>
        /// <param name="wordPath">word文件路径</param>
        /// <param name="htmlPath">html路径</param>
        /// <param name="htmlPageTitle">html页面title</param>
       Task ConvertToHtml(string wordPath, string htmlPath, string htmlPageTitle = null);
    }
}
