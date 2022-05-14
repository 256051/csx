using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public class ExcelReadOptionsProvier : IExcelReadOptionsProvier,ITransient
    {
        public const string ConfigurationKey = nameof(ExcelReadOptions);
        private ExcelReadOptions _options;
        public ExcelReadOptionsProvier(IOptions<ExcelReadOptions> options)
        {
            _options = options.Value;
        }

        public List<ReadItemOptions> GetReadItemsOptions()=> GetReadOptions().ReadItemsOptions;

        public ExcelReadOptions GetReadOptions() => _options;
    }
}
