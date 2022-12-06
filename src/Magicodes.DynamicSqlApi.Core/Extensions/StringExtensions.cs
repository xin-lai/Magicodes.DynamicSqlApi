using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Magicodes.DynamicSqlApi.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        //
        //   T:System.ArgumentException:
        //     Thrown if len is bigger that string's length
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        //
        // 摘要:
        //     Converts line endings in the string to System.Environment.NewLine.
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        //
        // 摘要:
        //     Gets a substring of a string from end of the string.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        //
        //   T:System.ArgumentException:
        //     Thrown if len is bigger that string's length
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by given separator.
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new string[1] { separator }, StringSplitOptions.None);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by given separator.
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new string[1] { separator }, options);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by System.Environment.NewLine.
        public static string[] SplitToLines(this string str)
        {
            return Split(str, Environment.NewLine);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by System.Environment.NewLine.
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return Split(str, Environment.NewLine, options);
        }

        //
        // 摘要:
        //     Converts PascalCase string to camelCase string.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   invariantCulture:
        //     Invariant culture
        //
        // 返回结果:
        //     camelCase of the string
        public static string ToCamelCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                if (!invariantCulture)
                {
                    return str.ToLower();
                }

                return str.ToLowerInvariant();
            }

            return (invariantCulture ? char.ToLowerInvariant(str[0]) : char.ToLower(str[0])) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts PascalCase string to camelCase string in specified culture.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   culture:
        //     An object that supplies culture-specific casing rules
        //
        // 返回结果:
        //     camelCase of the string
        public static string ToCamelCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLower(culture);
            }

            return char.ToLower(str[0], culture) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts given PascalCase/camelCase string to sentence (by splitting words by
        //     space). Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        //
        // 参数:
        //   str:
        //     String to convert.
        //
        //   invariantCulture:
        //     Invariant culture
        public static string ToSentenceCase(this string str, bool invariantCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", (Match m) => m.Value[0] + " " + (invariantCulture ? char.ToLowerInvariant(m.Value[1]) : char.ToLower(m.Value[1])));
        }

        //
        // 摘要:
        //     Converts given PascalCase/camelCase string to sentence (by splitting words by
        //     space). Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        //
        // 参数:
        //   str:
        //     String to convert.
        //
        //   culture:
        //     An object that supplies culture-specific casing rules.
        public static string ToSentenceCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", (Match m) => m.Value[0] + " " + char.ToLower(m.Value[1], culture));
        }

        //
        // 摘要:
        //     Converts string to enum value.
        //
        // 参数:
        //   value:
        //     String value to convert
        //
        // 类型参数:
        //   T:
        //     Type of enum
        //
        // 返回结果:
        //     Returns enum object
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        //
        // 摘要:
        //     Converts string to enum value.
        //
        // 参数:
        //   value:
        //     String value to convert
        //
        //   ignoreCase:
        //     Ignore case
        //
        // 类型参数:
        //   T:
        //     Type of enum
        //
        // 返回结果:
        //     Returns enum object
        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using (MD5 mD = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] array = mD.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    stringBuilder.Append(b.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }

        //
        // 摘要:
        //     Converts camelCase string to PascalCase string.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   invariantCulture:
        //     Invariant culture
        //
        // 返回结果:
        //     PascalCase of the string
        public static string ToPascalCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                if (!invariantCulture)
                {
                    return str.ToUpper();
                }

                return str.ToUpperInvariant();
            }

            return (invariantCulture ? char.ToUpperInvariant(str[0]) : char.ToUpper(str[0])) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts camelCase string to PascalCase string in specified culture.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   culture:
        //     An object that supplies culture-specific casing rules
        //
        // 返回结果:
        //     PascalCase of the string
        public static string ToPascalCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper(culture);
            }

            return char.ToUpper(str[0], culture) + str.Substring(1);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length. It adds a "..." postfix to end of the string if it's truncated. Returning
        //     string can not be longer than maxLength.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return str.TruncateWithPostfix(maxLength, "...");
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length. It adds given postfix to end of the string if it's truncated. Returning
        //     string can not be longer than maxLength.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
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

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }
    }
}
