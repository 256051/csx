using Framework.Core;
using Framework.Core.Dependency;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Excel.Npoi
{
    public class ReadItemOptionsProvider : IReadItemOptionsProvider, ITransient
    {
        private Dictionary<string, ReadItemOptions> _maps;
        private IExcelReadOptionsProvier _excelReadSettingsProvier;
        public ReadItemOptionsProvider(IExcelReadOptionsProvier excelReadSettingsProvier)
        {
            _excelReadSettingsProvier = excelReadSettingsProvier;
            _maps = new Dictionary<string, ReadItemOptions>();
        }

        public ReadItemOptions GetReadItemOptions<T>() where T : class, new()
        {
            var readItemsOptions = _excelReadSettingsProvier.GetReadItemsOptions();
            readItemsOptions = readItemsOptions.Where(w => w.EntityType == typeof(T).FullName).ToList();
            if (readItemsOptions.Count() == 0)
                throw new FrameworkException($"{typeof(T).FullName} does not exist excel map please check it again!");
            if (readItemsOptions.Count() > 1)
                throw new FrameworkException($"{typeof(T).FullName} already have a excel map please check it again!");
            var readItemOptions = readItemsOptions.FirstOrDefault();
            var readOptions = _excelReadSettingsProvier.GetReadOptions();
            readItemOptions.FilledByExcelReadOptions(readOptions);
            CheckReadItemOptions<T>(readItemOptions);
            return _maps.GetOrAdd(typeof(T).FullName, () => readItemOptions);
        }

        private void CheckReadItemOptions<T>(ReadItemOptions readItemOptions) where T : class, new()
        {
            if (readItemOptions.MapOptions.Count == 0)
                throw new FrameworkException($"please set MapOptions for {typeof(T).FullName} in excel map file!");
            if (readItemOptions.StartRowIndex == null)
                throw new FrameworkException($"please set StartRowIndex for {typeof(T).FullName} in excel map file!");
            if (readItemOptions.EndColumnIndex == null)
                throw new FrameworkException($"please set EndColumnIndex for {typeof(T).FullName} in excel map file!");
        }
    }
}
