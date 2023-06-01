using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.Bindings.Tools
{
    public readonly struct ConvertableString {
        public readonly string Data;

        public ConvertableString(string data) => this.Data = data;
        public ConvertableString(IEnumerable<int> array) => this.Data = String.Join(ConvertableStringHelper.Separator, array);
        public ConvertableString(IEnumerable<long> array) => this.Data = String.Join(ConvertableStringHelper.Separator, array);
        public ConvertableString(IEnumerable<float> array) => this.Data = String.Join(ConvertableStringHelper.Separator, array);
        public ConvertableString(IEnumerable<double> array) => this.Data = String.Join(ConvertableStringHelper.Separator, array);
        public ConvertableString(IEnumerable<bool> array) => this.Data = String.Join(ConvertableStringHelper.Separator, array);

        public static implicit operator ConvertableString(string v) => new ConvertableString(v);
        public static implicit operator string(ConvertableString v) => v.Data;
        public static implicit operator int(ConvertableString v) => ConvertableStringHelper.StringToInt(v.Data);
        public static implicit operator long(ConvertableString v) => ConvertableStringHelper.StringToLong(v.Data);
        public static implicit operator float(ConvertableString v) => ConvertableStringHelper.StringToFloat(v.Data);
        public static implicit operator double(ConvertableString v) => ConvertableStringHelper.StringToDouble(v.Data);
        public static implicit operator bool(ConvertableString v) => ConvertableStringHelper.StringToBool(v.Data);
    }

    public static class ConvertableStringHelper {
        public const string Separator = " ";
        private static char[] _separator = new char[] { Separator[0] };

        public static List<int> ToListInt(this ConvertableString v) => ToList(v.Data, StringToInt);
        public static List<long> ToListLong(this ConvertableString v) => ToList(v.Data, StringToLong);
        public static List<float> ToListFloat(this ConvertableString v) => ToList(v.Data, StringToFloat);
        public static List<double> ToListDouble(this ConvertableString v) => ToList(v.Data, StringToDouble);
        public static List<bool> ToListBool(this ConvertableString v) => ToList(v.Data, StringToBool);

        private static List<T> ToList<T>(string value, Func<string, T> converter) {
            var list = new List<T>();
            foreach (var val in value.Split(_separator, StringSplitOptions.RemoveEmptyEntries)) {
                list.Add(converter(val));
            }
            return list;
        }

        public static int StringToInt(this string value) => int.TryParse(value, out var result) ? result : 0;
        public static long StringToLong(this string value) => long.TryParse(value, out var result) ? result : 0L;
        public static float StringToFloat(this string value) => float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 0f;
        public static double StringToDouble(this string value) => double.TryParse(value, NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 0d;
        public static bool StringToBool(this string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }
            switch (value) {
                case "1":
                case "+":
                    return true;

                case "0":
                case "-":
                    return false;
            }

            var lower = value.ToLower();
            switch (lower) {
                case "true":
                    return true;

                case "false":
                    return false;

                default:
                    return int.TryParse(lower, out _);
            }
        }
    }
}