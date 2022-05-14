using NPOI.SS.UserModel;
using System;

namespace Framework.Excel.Npoi
{
    public static class NpoiExtensions
    {
        public static object Extract(this ICell cell, Type valueType)
        {
            if (valueType == typeof(string))
            {
                try
                {
                    return cell.StringCellValue;
                }
                catch (InvalidOperationException)
                {
                    //兼容excel中的身份证之类的被解析成numeric
                    return cell.NumericCellValue.ToString();
                }
            }
            if (valueType == typeof(DateTime))
            {
                return cell.DateCellValue;
            }
            if (valueType == typeof(double))
            {
                return cell.NumericCellValue;
            }
            if (valueType == typeof(int))
            {
                return (int)cell.NumericCellValue;
            }
            if (valueType == typeof(IRichTextString))
            {
                return cell.RichStringCellValue;
            }
            if (valueType == typeof(bool))
            {
                return cell.BooleanCellValue;
            }
            if (valueType == typeof(Guid))
            {
                return new Guid(cell.StringCellValue);
            }
            throw new NotSupportedException();
        }
    }
}
