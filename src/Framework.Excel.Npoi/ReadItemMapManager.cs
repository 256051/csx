using Framework.Core.Dependency;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public class ReadItemMapManager : IReadItemMapManager,ISingleton
    {
        private Lazy<IDictionary<string, List<CellReadMapItem>>> CellMap { get; }
        private IExcelReadOptionsProvier _excelReadOptionsProvier;

        public ReadItemMapManager(IExcelReadOptionsProvier excelReadOptionsProvier)
        {
            _excelReadOptionsProvier = excelReadOptionsProvier;
            CellMap = new Lazy<IDictionary<string, List<CellReadMapItem>>>(CreateCellMaps, true);
        }

        protected virtual IDictionary<string, List<CellReadMapItem>> CreateCellMaps()
        {
            var maps = new Dictionary<string, List<CellReadMapItem>>();
            var options=_excelReadOptionsProvier.GetReadItemsOptions();
            options.ForEach(option =>
            {
                var typeName = option.EntityType;
                if (!maps.ContainsKey(typeName))
                {
                    maps.Add(typeName, CellReadMapItem.FilledByOptions(option.MapOptions));
                }
            });
            return maps;
        }

        public List<CellReadMapItem> GetMaps<T>() where T : class, new()
        {
            return CellMap.Value.GetOrDefault(typeof(T).FullName);
        }

        public void UpdateMap<T>(ReadItemOptions readItemOptions,IRow headerRow) where T : class, new()
        {
            var propertyCount =typeof(T).GetProperties().Length;
            int? trackedStartIndex = null;
            for (var index = 0; index < int.MaxValue; index++)
            {
                if (trackedStartIndex != null)
                {
                    var endColumnIndex = propertyCount + trackedStartIndex.Value;
                    if (readItemOptions.EndColumnIndex.HasValue && index > Math.Max(endColumnIndex, readItemOptions.EndColumnIndex.Value))
                        break;
                }
                var cell = headerRow.GetCell(index);
                if (cell == null)
                    continue;
                trackedStartIndex = trackedStartIndex ?? index;
                var cellHeaderName = cell.StringCellValue;

                var maps = GetMaps<T>();
                maps.ForEach(map =>
                {
                    if (map.CellName.Equals(cellHeaderName))
                    {
                        map.CellIndex = index;
                        map.PropertyType = typeof(T).GetProperty(map.PropertyName)?.PropertyType;
                    }
                });
            }
        }
    }
}
