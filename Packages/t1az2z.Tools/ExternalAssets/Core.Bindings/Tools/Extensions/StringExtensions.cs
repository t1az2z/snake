using System;
using System.Text;
using System.Text.RegularExpressions;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif
    public static class StringExtensions {
        /// <summary>
        /// Returns whether or not the specified string is contained with this string
        /// Credits to JaredPar http://stackoverflow.com/questions/444798/case-insensitive-containsstring/444818#444818
        /// </summary>
        public static bool Contains(this string source, string toCheck, StringComparison comp) {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string RemoveEscapeChars(this string source, char escapechar) {
            return source.Replace($"{escapechar}{escapechar}", $"{escapechar}");
        }

        /// <summary>
        /// Returns the Nth index of the specified character in this string
        /// </summary>
        public static int IndexOfNth(this string str, char c, int n) {
            int s = -1;

            for (int i = 0; i < n; i++) {
                s = str.IndexOf(c, s + 1);
                if (s == -1)
                    break;
            }

            return s;
        }

        /// <summary>
        /// Returns true if this string is null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Nomalizes this string by replacing all '/' with '\' and returns the normalized string instance
        /// </summary>
        public static string NormalizePath(this string input) {
            return input.NormalizePath('/', '\\');
        }

        /// <summary>
        /// Normalizes this string by replacing all 'from's by 'to's and returns the normalized instance
        /// Ex: "path/to\dir".NormalizePath('/', '\\') => "path\\to\\dir"
        /// </summary>
        public static string NormalizePath(this string input, char from, char to) {
            return input?.Replace(from, to);
        }

        public static bool ParseIntFromBegin(this string str, out int result, bool ignoreLeadingNonDigits = false) {
            result = 0;
            if (string.IsNullOrEmpty(str)) {
                return false;
            }

            var startIndex = 0;
            if (ignoreLeadingNonDigits) {
                while ((startIndex < str.Length) && !char.IsDigit(str[startIndex])) {
                    ++startIndex;
                }

                if ((startIndex == str.Length)) {
                    return false;
                }
            }

            if (!char.IsDigit(str[startIndex])) {
                return false;
            }

            for (int i = startIndex; i <= str.Length - 1; ++i) {
                if (!char.IsDigit(str[i])) {
                    result = Convert.ToInt32(str.Substring(startIndex, i - startIndex));
                    return true;
                }
            }

            result = Convert.ToInt32(str);
            return true;
        }

        public static bool ParseIntFromEnd(this string str, out int result) {
            result = 0;
            if (string.IsNullOrEmpty(str) || !char.IsDigit(str[str.Length - 1])) {
                return false;
            }

            for (int i = str.Length - 1; i >= 0; --i) {
                if (!char.IsDigit(str[i])) {
                    result = Convert.ToInt32(str.Substring(i + 1));
                    return true;
                }
            }

            result = Convert.ToInt32(str);
            return true;
        }

        /// <summary>
        /// Removes the last occurance of the specified string from this string.
        /// Returns the modified version.
        /// </summary>
        public static string RemoveLastOccurance(this string s, string what) {
            return s?.Substring(0, s.LastIndexOf(what));
        }

        /// <summary>
        /// Replaces the character specified by the passed index with newChar and returns the new string instance
        /// </summary>
        public static string ReplaceAt(this string input, int index, char newChar) {
            if (input == null) {
                throw new ArgumentNullException("input");
            }

            var builder = new StringBuilder(input);
            builder[index] = newChar;
            return builder.ToString();
        }

        /// <summary>
        /// Ex: "thisIsCamelCase" -> "this Is Camel Case"
        /// Credits: http://stackoverflow.com/questions/155303/net-how-can-you-split-a-caps-delimited-string-into-an-array
        /// </summary>
        public static string SplitCamelCase(this string input) {
            return Regex.Replace(input, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        /// <summary>
        /// Ex: "thisIsCamelCase" -> "This Is Camel Case"
        /// </summary>
        public static string SplitPascalCase(this string input) {
            return input?.SplitCamelCase().ToUpperAt(0);
        }

        /// <summary>
        /// "tHiS is a sTring TesT" -> "This Is A String Test"
        /// Credits: http://extensionmethod.net/csharp/string/topropercase
        /// </summary>
        public static string ToProperCase(this string text) {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(text);
        }

        /// <summary>
        /// Eg MY_INT_VALUE => MyIntValue
        /// </summary>
        public static string ToTitleCase(this string input) {
            var builder = new StringBuilder();
            for (int i = 0; i < input.Length; i++) {
                var current = input[i];
                if (current == '_' && i + 1 < input.Length) {
                    var next = input[i + 1];
                    if (char.IsLower(next))
                        next = char.ToUpper(next);
                    builder.Append(next);
                    i++;
                }
                else { 
                    builder.Append(i == 0 ? char.ToUpper(current) : current);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Uppers the character specified by the passed index and returns the new string instance
        /// </summary>
        public static string ToUpperAt(this string input, int index) {
            return input.ReplaceAt(index, char.ToUpper(input[index]));
        }

        /// <summary>
        /// Removes the type extension. ex "Medusa.mp3" => "Medusa"
        /// </summary>
        public static string WithoutExtension(this string s) {
            return s?.Substring(0, s.LastIndexOf('.'));
        }
    }

    public static class StringSizeFormatter {
        public static string SetSizeInPercent(this string text, int size) => string.Format(SizePercentFormat, text, size.ToString());
        
        private const string SizePercentFormat = "<size={1}%>{0}</size>";
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif