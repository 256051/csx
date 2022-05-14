using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Framework.Word.Npoi.Tests
{
    public class WordConverterTest: NpoiTest
    {
        private IWordConverter _wordConverter;
        public WordConverterTest()
        {
            _wordConverter=ServiceProvider.GetRequiredService<IWordConverter>();
        }

        [Fact]
        public void HtmlConvert()
       {
            _wordConverter.ConvertToHtml(
                $"{AppDomain.CurrentDomain.BaseDirectory}word\\������ð�ȫ����������2021-09-15.docx",
                $"{AppDomain.CurrentDomain.BaseDirectory}html\\������ð�ȫ����������2021-09-15-07.html",
                "������ð�ȫ����������2021-09-15"
                );
        }
    }
}
