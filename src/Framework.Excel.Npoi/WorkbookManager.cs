using Framework.Core;
using Framework.Core.Dependency;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace Framework.Excel.Npoi
{
    public class WorkbookManager: IWorkbookManager,IScoped,IDisposable
    {
        private IWorkbook _workbook;
        public (IWorkbook, ISheet) CreateSheet(string workbookType, string sheetName,bool requireNew=false)
        {
            ThrowIfDisposed();
            if (_workbook == null || requireNew)
            {
                _workbook = CreateWorkbook(workbookType);
            }
            return (_workbook, _workbook.CreateSheet(sheetName));
        }

        public IWorkbook CreateWorkbook(string workbookType)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workbookType))
                throw new ArgumentNullException(nameof(workbookType));
            IWorkbook workBook;
            switch (workbookType)
            {
                case "HSSFWorkbook": workBook = new HSSFWorkbook(); break;
                case "XSSFWorkbook": workBook = new XSSFWorkbook(); break;
                case "SXSSFWorkbook": workBook = new SXSSFWorkbook(); break;
                default: workBook = new XSSFWorkbook(); break;
            }
            return workBook;
        }

        public (IWorkbook, ISheet) GetSheet(string workbookType, Stream stream, string sheetName, bool requireNew = false)
        {
            ThrowIfDisposed();
            if (_workbook == null || requireNew)
            {
                _workbook = GetWorkbook(workbookType, stream);
            }

            ISheet sheet;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheet = _workbook.GetSheetAt(0);
            }
            else
            {
                sheet = _workbook.GetSheet(sheetName);
            }
            if (sheet == null)
                throw new FrameworkException($"sheet named {sheetName} dose not existed");
            return (_workbook, sheet);
        }

        public IWorkbook GetWorkbook(string workbookType, Stream stream)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workbookType))
                throw new ArgumentNullException(nameof(workbookType));
            if (stream.Length == 0)
                throw new ArgumentNullException(nameof(stream));
            IWorkbook workBook;
            switch (workbookType)
            {
                case "HSSFWorkbook": workBook = new HSSFWorkbook(stream); break;
                case "XSSFWorkbook": workBook = new XSSFWorkbook(stream); break;
                case "SXSSFWorkbook":
                    {
                        workBook = new SXSSFWorkbook(new XSSFWorkbook(stream));
                    }; break;
                default: workBook = new XSSFWorkbook(stream); break;
            }
            return workBook;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
            }
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
