using Framework.Core.Dependency;
using Framework.Expressions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Excel.Npoi
{
    public class ExcelReader : IExcelReader, IScoped
    {
        private IReadItemOptionsProvider _readItemOptionsProvider;
        private IReadItemMapManager _readItemMapManager;
        private IExpressionsReflectionHelper _reflectionHelper;
        private IWorkbookManager _workbookManager;
        public ExcelReader(
            IReadItemOptionsProvider readItemOptionsProvider,
            IReadItemMapManager readItemMapManager,
            IExpressionsReflectionHelper reflectionHelper,
            IWorkbookManager workbookManager)
        {
            _readItemOptionsProvider = readItemOptionsProvider;
            _readItemMapManager = readItemMapManager;
            _reflectionHelper = reflectionHelper;
            _workbookManager = workbookManager;
        }

        public IList<T> Read<T>(Stream stream,Action<ReadOption> option = null) where T : class, new()
        {
            var readOption = new ReadOption();
            option?.Invoke(readOption);
            var readItemOptions = _readItemOptionsProvider.GetReadItemOptions<T>();
            (var workbook, var sheet) = _workbookManager.GetSheet(readItemOptions.WorkbookType, stream, readItemOptions.SheetName);
            var failureCount = 0;
            var result = new List<T>();
            for (var index = 0; index < int.MaxValue; index++)
            {
                if (failureCount == readItemOptions.ReadFailed.Value)
                    break;
                var row = sheet.GetRow(index);

                if (index < readItemOptions.StartRowIndex) continue;

                var canStop = readOption.StopRead?.Invoke(row);
                if (canStop.HasValue && canStop.Value)
                {
                    failureCount++;
                    continue;
                }

                //读取标题行,更新映射关系
                if (index == readItemOptions.StartRowIndex)
                {
                    _readItemMapManager.UpdateMap<T>(readItemOptions, row);
                    continue;
                }

                var instance = new T();
                foreach (var map in _readItemMapManager.GetMaps<T>())
                {
                    if (map.CellIndex == null)
                    {
                        continue;
                    }
                    var cell = row.GetCell(map.CellIndex.Value);
                    if (map.PropertyType != null && !string.IsNullOrEmpty(map.PropertyName))
                    {
                        if (cell == null)
                        {
                            continue;
                        }
                        _reflectionHelper.PropertySetValue(instance, map.PropertyName, cell.Extract(map.PropertyType));
                    }
                }
                result.Add(instance);
            }
            return result;
        }
    }
}
