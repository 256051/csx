using Framework.Core;
using Framework.Core.Dependency;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Framework.Excel.Npoi
{
    public class WriteItemOptionsProvider: IWriteItemOptionsProvider, ITransient
    {
        private Dictionary<string, WriteItemOptions> _maps;
        private IExcelWriteOptionsProvier _excelWriteOptionsProvier;
        public WriteItemOptionsProvider(IExcelWriteOptionsProvier excelWriteOptionsProvier)
        {
            _excelWriteOptionsProvier = excelWriteOptionsProvier;
            _maps = new Dictionary<string, WriteItemOptions>();
        }

        public WriteItemOptions GetWriteItemOptions<T>() where T : class, new()
        {
            return _maps.GetOrAdd(typeof(T).FullName, () => {
                var writeItemsOptions = _excelWriteOptionsProvier.GetWriteItemsOptions();
                writeItemsOptions = writeItemsOptions
                    .Where(w => w.EntityType == typeof(T).FullName)
                    .ToList();
                if (writeItemsOptions.Count() == 0)
                    throw new ArgumentException($"{typeof(T).FullName} does not exist excel map please check it again!");
                var writeItemOptions = writeItemsOptions.FirstOrDefault();
                var writeOptions = _excelWriteOptionsProvier.GetWriteOptions();
                writeItemOptions.FilledByExcelWriteOptions(writeOptions);
                CheckWriteItemOptions<T>(writeItemOptions);
                return writeItemOptions;
            });
        }

        private void CheckWriteItemOptions<T>(WriteItemOptions writeItemOptions) where T : class, new()
        {
            if (writeItemOptions.MapOptions.Count == 0)
                throw new FrameworkException($"please set MapOptions for {typeof(T).FullName} in excel map file for write options!");
            if (!writeItemOptions.StartRowIndex.HasValue)
                throw new FrameworkException($"please set StartRowIndex for {typeof(T).FullName} in excel map file for write options!");
            if (!writeItemOptions.StartCellIndex.HasValue)
                throw new FrameworkException($"please set StartCellIndex for {typeof(T).FullName} in excel map file for write options!");
        }
    }
}
