using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// 安全的执行一个字符串非空操作(无返回值)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="act"></param>
        public static void IfNotNullAndEmpty(this string str, Action<string> act)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            act(str);
        }

        public static void IfNotNullAndEmpty(this string str, Action<string> act, Action actException)
        {
            if (string.IsNullOrEmpty(str))
            {
                actException?.Invoke();
            }
            act?.Invoke(str);
        }

        /// <summary>
        /// 判断该字符串是否为空或者是否为空引用
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 判断该字符串是否为空或者为null
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 安全的执行一个字符串非空操作(有返回值)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="str"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult IfNotNullAndEmpty<TResult>(this string str, Func<string, TResult> func)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(TResult);
            }
            return func(str);
        }

        /// <summary>
        /// 安全的执行一个字符串非空操作(有返回值)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="str"></param>
        /// <param name="func"></param>
        /// <param name="rFunc"></param>
        /// <returns></returns>
        public static TResult IfNotNullAndEmpty<TResult>(this string str, Func<string, TResult> func, Func<string, TResult> rFunc)
        {
            if (string.IsNullOrEmpty(str))
            {
                return rFunc(str);
            }
            return func(str);
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct
        {
            var value = default(TEnum);
            str.IfNotNullAndEmpty(f =>
            {
                if (Enum.TryParse(str, out value))
                    return value;
                return default(TEnum);
            });
            return value;
        }

        /// <summary>
        /// 安全将字符串转换成枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static bool SafeToEnum<TEnum>(this string str) where TEnum : struct
        {
            return str.IfNotNullAndEmpty(f =>
            {
                var eValue = str.ToEnum<TEnum>();
                if (Enum.IsDefined(typeof(TEnum), eValue))
                {
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        ///字符串转换成枚举失败后执行传入Action
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="str"></param>
        /// <param name="act"></param>
        public static void NotInEnum<TEnum>(this string str, Action act) where TEnum : struct
        {
            str.IfNotNullAndEmpty(s =>
            {
                if (!str.SafeToEnum<TEnum>())
                {
                    act();
                }
            });
        }

        /// <summary>
        /// 将字符串按指定分隔符切割成字符串数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Base64转Stream
        /// </summary>
        /// <returns></returns>
        public static Stream Base64ToStream(this string base64Str)
        {
            base64Str.IfNotNullAndEmpty(str =>
            {
                byte[] bytes = Convert.FromBase64String(str);
                return new MemoryStream(bytes);
            });
            return null;
        }

        /// <summary>
        /// 反向截取指定位数的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num">位数</param>
        /// <returns></returns>
        public static string ObeySubString(this string str, int num)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            else
            {
                var index = str.Length - num;
                if (index < 0)
                {
                    index = 0;
                }
                return str.Substring(index);
            }
        }

        /// <summary>
        /// 确保字符串已指定字符结尾
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str.EndsWith(c.ToString(), comparisonType))
            {
                return str;
            }
            return str + c;
        }

        /// <summary>
        /// 按单字节字符串向左填充长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string PadLeftWhileDouble(this string input, int length, char paddingChar = '\0')
        {
            var singleLength = GetSingleLength(input);
            return input.PadLeft(length - singleLength + input.Length, paddingChar);
        }

        private static int GetSingleLength(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            return Regex.Replace(input, @"[^\x00-\xff]", "aa").Length;//计算得到该字符串对应单字节字符串的长度
        }

        /// <summary>
        /// 按单字节字符串向右填充长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string PadRightWhileDouble(this string input, int length, char paddingChar = '\0')
        {
            var singleLength = GetSingleLength(input);
            return input.PadRight(length - singleLength + input.Length, paddingChar);
        }

        /// <summary>
        /// 向左截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len">截取多少位</param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            Check.NotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// 向左截取最大不超过指定位数的字符并以_开头
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength">最大截取多少位</param>
        /// <returns></returns>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "_");
        }

        /// <summary>
        /// 向左截取最大不超过指定位数的字符并以指定字符串开头
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength">最大截取多少位</param>
        /// <param name="postfix">开头字符</param>
        /// <returns></returns>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            return postfix+str.Left(maxLength - postfix.Length) ;
        }

        /// <summary>
        /// 去除字符串的bom头
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="targetEncoding">字符串的编码格式</param>
        /// <param name="encoding">希望输出的字符串以什么编码格式编码</param>
        /// <returns></returns>
        public static string WithNobom(this string str, Encoding targetEncoding=null, Encoding encoding = null)
        {
            if (targetEncoding == null)
            {
                targetEncoding = Encoding.UTF8;
            }

            var bytes = targetEncoding.GetBytes(str);
            if (bytes == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var hasBom = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;

            if (hasBom)
            {
                return encoding.GetString(bytes, 3, bytes.Length - 3);
            }
            else
            {
                return encoding.GetString(bytes);
            }
        }

        public static string EnsureLeadingSlash(this string url)
        {
            if (url != null && !url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }
    }
}
