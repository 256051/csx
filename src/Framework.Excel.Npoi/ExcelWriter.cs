using Framework.Core.Dependency;
using Framework.Expressions;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Excel.Npoi
{
    public class ExcelWriter: IExcelWriter, IScoped
    {
        private IWriteItemOptionsProvider _writeItemOptionsProvider;
        private IExpressionsReflectionHelper _reflectionHelper;
        private IServiceScopeFactory _scopeFactory;
        private IWorkbookManager _workbookManager;
        public ExcelWriter(
            IWriteItemOptionsProvider writeItemOptionsProvider,
            IExpressionsReflectionHelper reflectionHelper, IServiceScopeFactory scopeFactory, IWorkbookManager workbookManager)
        {
            _writeItemOptionsProvider = writeItemOptionsProvider;
            _reflectionHelper = reflectionHelper;
            _scopeFactory = scopeFactory;
            _workbookManager = workbookManager;
        }

        private IWorkbook WriteWorkbook<T>(IEnumerable<T> data,bool requireNew) where T : class,new()
        {
            var writeItemOptions = _writeItemOptionsProvider.GetWriteItemOptions<T>();
            (var workbook, var sheet) = _workbookManager.CreateSheet(writeItemOptions.WorkbookType, writeItemOptions.SheetName, requireNew);
            var rowIndex = writeItemOptions.StartRowIndex.Value;
            var titles = writeItemOptions.MapOptions.Select(s => s.CellName);
            var propertyNames = writeItemOptions.MapOptions.Select(s => s.PropertyName);
            WriteHeaderRow(ref rowIndex, sheet, writeItemOptions.StartCellIndex.Value, writeItemOptions.DateFormat, titles);
            WriteBodyRows(ref rowIndex, sheet, writeItemOptions.StartCellIndex.Value, propertyNames, writeItemOptions.DateFormat, data);
            SetWidth(sheet, writeItemOptions.MapOptions.Select(s => s.CellWidth).ToList());
            return workbook;
        }

        public ExcelWriterContext Write<T>(IEnumerable<T> data) where T : class, new()
        {
            return new ExcelWriterContext(WriteWorkbook(data, requireNew: false), this);
        }

        public void Write<T>(IEnumerable<T> data,string filePath) where T : class, new()=> WriteWorkbook(data, requireNew:true).SaveAs(filePath);

        /// <summary>
        /// 写入标题行到sheet
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="sheet"></param>
        /// <param name="cellStartIndex"></param>
        /// <param name="dateFormat"></param>
        /// <param name="titles"></param>
        private void WriteHeaderRow(ref int rowIndex,ISheet sheet,int cellStartIndex,string dateFormat, IEnumerable<string> titles)
        {
            var headerRow = sheet.CreateRow(rowIndex);
            foreach (var title in titles)
            {
                var cell = headerRow.CreateCell(cellStartIndex);
                SetCellValue(cell, title, dateFormat);
                cellStartIndex++;
            }
            rowIndex++;
        }

        /// <summary>
        /// 写入数据到内容行
        /// </summary>
        private void WriteBodyRows<T>(ref int rowIndex, ISheet sheet, int cellStartIndex, IEnumerable<string> propertyNames, string dateFormat, IEnumerable<T> data) where T : class, new()
        {
            var cellIndex = cellStartIndex;
            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowIndex);
                foreach (var propertyName in propertyNames)
                {
                    var cell = row.CreateCell(cellIndex);
                    SetCellValue(cell, _reflectionHelper.PropertyGetValue(item, propertyName), dateFormat);
                    cellIndex++;
                }
                cellIndex = cellStartIndex;
                rowIndex++;
            }
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <param name="value">值</param>
        /// <param name="dateFromat">时间格式</param>
        private void SetCellValue(ICell cell, object value,string dateFromat)
        {
            if (value is string)
            {
                cell.SetCellValue(value.ToString());
                return;
            }
            if (value is double)
            {
                cell.SetCellValue(value.To<double>());
                return;
            }
            if (value is int)
            {
                cell.SetCellValue(value.To<int>());
                return;
            }
            if (value is long)
            {
                cell.SetCellValue(value.To<long>());
                return;
            }
            if (value is float)
            {
                cell.SetCellValue(value.To<float>());
                return;
            }
            if (value is DateTime)
            {
                cell.SetCellValue(value.To<DateTime>().ToString(dateFromat));
                return;
            }
            if (value is bool)
            {
                cell.SetCellValue((bool)value);
                return;
            }
            if (value is null)
            {
                cell.SetCellValue(string.Empty);
                return;
            }
            else {
                throw new ArgumentException("unknown data type");
            }
        }

        /// <summary>
        /// 设置单元格宽度
        /// </summary>
        private void SetWidth(ISheet sheet, List<int?> widths)
        {
            for (var index = 0; index < widths.Count(); index++)
            {
                var width = widths.ElementAt(index);
                if (width.HasValue)
                {
                    sheet.SetColumnWidth(index, width.Value);
                }
                else
                {
                    sheet.AutoSizeColumn(index, true);
                }
            }
        }
    }
}
