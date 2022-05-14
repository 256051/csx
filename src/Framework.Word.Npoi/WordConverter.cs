using DocumentFormat.OpenXml.Packaging;
using Framework.Core.Dependency;
using OpenXmlPowerTools;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Framework.Word.Npoi
{
    public class WordConverter : IWordConverter, ISingleton
    {
        public async Task<byte[]> ConvertToHtmlAsync(byte[] byteArray, string htmlPageTitle=default)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await memoryStream.WriteAsync(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                {
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = htmlPageTitle,
                        ImageHandler = imgInfo =>
                        {
                            using (var ms = new MemoryStream())
                            {
                                imgInfo.Bitmap.Save(ms, imgInfo.Bitmap.RawFormat);
                                byte[] imageBytes = new byte[ms.Length];
                                ms.Position = 0;
                                ms.Read(imageBytes, 0, (int)ms.Length);
                                var baseString=Convert.ToBase64String(imageBytes);
                                XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, $"data:{imgInfo.ContentType};base64,{baseString}"),imgInfo.ImgStyleAttribute);
                                return img;
                            }
                        }
                    };
                    string htmlContent = HtmlConverter.ConvertToHtml(doc, settings).ToStringNewLineOnAttributes();
                    return Encoding.UTF8.GetBytes(htmlContent);
                }
            }
        }

        public async Task ConvertToHtml(string wordPath, string htmlPath, string htmlPageTitle = null)
        {
            if (!File.Exists(wordPath))
                throw new ArgumentException($"the word file path {wordPath} not exists");
            if (!File.Exists(htmlPath))
            {
                var directory = Directory.GetParent(htmlPath).FullName;
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                var bytes = await ConvertToHtmlAsync(File.ReadAllBytes(wordPath), htmlPageTitle);
                using (var fs = File.Create(htmlPath))
                {
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            else {
                throw new ArgumentException($"the html file already exists");
            }
        }
    }
}
