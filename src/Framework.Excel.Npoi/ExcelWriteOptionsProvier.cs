using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public class ExcelWriteOptionsProvier : IExcelWriteOptionsProvier,ITransient
    {
        public const string ConfigurationKey =nameof(ExcelWriteOptions);
        private ExcelWriteOptions _options;
        public ExcelWriteOptionsProvier(IOptions<ExcelWriteOptions> options)
        {
            _options = options.Value;
        }
        public List<WriteItemOptions> GetWriteItemsOptions() => GetWriteOptions().WriteItemsOptions;

        public ExcelWriteOptions GetWriteOptions()=> _options;
    }
}
