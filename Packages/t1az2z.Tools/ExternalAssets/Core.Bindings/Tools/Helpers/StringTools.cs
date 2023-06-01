using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Helpers {
#endif
    public static partial class StringTools {
        public static readonly string ThousandLetter = "K";
        public static readonly string MillionLetter = "M";

        private const int IntToStringLength = 1000;
        private const int IntToStringTimeLength = 60;

        private static string[] _intToString;
        private static string[] _oneNumSigns = null;
        private static string[] _twoNumSigns = null;
        private static string[] _threeNumSigns = null;
        private const int MaxTwoNumSigns = 99;
        private const int MaxThreeNumSigns = 999;
        private static HashSet<string> _preallocatedHashset = null;

        private static CultureInfo _culture = new CultureInfo("ru-RU") { NumberFormat = { NumberGroupSeparator = " ", }, };

        static StringTools() {
            _intToString = new string[IntToStringLength];
            for (int i = 0; i < IntToStringLength; ++i) {
                _intToString[i] = i.ToString("N0", _culture);
            }

            _oneNumSigns = CreateNumSigns(9, "0");
            _twoNumSigns = CreateNumSigns(MaxTwoNumSigns, "00");
            _threeNumSigns = CreateNumSigns(MaxThreeNumSigns, "000");

            _preallocatedHashset = new HashSet<string>(_twoNumSigns);
            _preallocatedHashset.Clear();
        }

        private static string[] CreateNumSigns(int maxLen, string formatter) {
            var len = maxLen + 1;
            var result = new string[len];
            for (var i = 0; i < len; i++) {
                result[i] = i.ToString(formatter);
            }

            return result;
        }

        public static HashSet<string> GetTempPreallocatedHashSet() {
            _preallocatedHashset.Clear();
            return _preallocatedHashset;
        }

        public static string NumToTwoSignString(int value) {
            value = Mathf.Clamp(value, 0, MaxTwoNumSigns);
            return _twoNumSigns[value];
        }

        public static string SignedNumToThreeSignString(int value) {
            return (value >= 0) ? $"+{NumToThreeSignString(value)}" : $"-{NumToThreeSignString(-value)}";
        }

        public static string NumToThreeSignString(int value) {
            value = Mathf.Clamp(value, 0, MaxThreeNumSigns);
            return _threeNumSigns[value];
        }

        public static string NumToString(uint value) {
            if (value < 10) {
                return _oneNumSigns[value];
            }

            if (value < 100) {
                return _twoNumSigns[value];
            }

            if (value < 1000) {
                return _threeNumSigns[value];
            }

            //fallback
            return value.ToString();
        }

        public static string IntToString(int integer) {
            if (integer >= 0 && integer < IntToStringLength) {
                return _intToString[integer];
            }

            return integer.ToString("N0", _culture);
        }

        public static string LongToString(long longInt) {
            if (longInt >= 0 && longInt < IntToStringLength) {
                return _intToString[longInt];
            }

            return longInt.ToString("N0", _culture);
        }

        public static string IntToTimeString(int integer) {
            if (integer >= 0 && integer < IntToStringTimeLength) {
                return _twoNumSigns[integer];
            }

            return integer.ToString("00");
        }

        public static string ExtractCommand(ref string source, char marker = '@') {
            if (string.IsNullOrEmpty(source)) {
                return "";
            }

            if (source.Length < 2) {
                return "";
            }

            var input = source;
            var begin = FindNext(0);
            if (begin == -1) {
                return "";
            }

            var end = FindNext(begin + 1);
            if ((begin == end) || (end == -1)) {
                return "";
            }

            var token = input.Substring(begin + 1, end - begin - 1);
            source = source.Remove(begin, end - begin + 1);
            return token;

            int FindNext(int pos) {
                while (pos < input.Length) {
                    if ((input[pos] == marker) && !SafeCheck(pos + 1)) {
                        return pos;
                    }

                    ++pos;
                }

                return -1;
            }

            bool SafeCheck(int pos) {
                if ((pos < 0) || (pos >= input.Length)) {
                    return false;
                }

                return (input[pos] == marker);
            }
        }

        public static string GetToken(ref string source, string def = "") {
            var token = def;
            var index = source.IndexOf(" ");
            if (index != -1) {
                token = source.Substring(0, index);
                source = source.Substring(index + 1);
            }
            else {
                if (source.Length > 0) {
                    token = source.Trim();
                    source = "";
                }

                if (token == "") {
                    return def;
                }
            }

            return token;
        }
        
        public static string FormatNumberLiterals(long number) {
            if (number >= 100000000) {
                return $"{IntToString(Mathf.FloorToInt(number / 1000000f))}{MillionLetter}";
            }
            if (number >= 1000000) {
                return $"{Mathf.Floor(number / 100000f) / 10:0.0}{MillionLetter}";
            }
            if (number >= 100000) {
                return $"{IntToString(Mathf.FloorToInt(number / 1000f))}{ThousandLetter}";
            }
            if (number >= 1000) {
                return $"{Mathf.Floor(number / 100f) / 10:0.0}{ThousandLetter}";
            }
            return LongToString(number);
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif