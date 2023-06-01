using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Core.Bindings.Tools.Extensions
{
    public static class UnsortedExtensions {
        public static bool FastSequenceEqual<T>(this IList<T> a, IList<T> b) {
            if ((a == null && b != null) || (a != null && b == null)) {
                return false;
            }

            if (a == null && b == null) {
                return true;
            }

            if (a.Count != b.Count) {
                return false;
            }

            for (int i = 0; i < a.Count; i++) {
                if (!a[i].Equals(b[i])) {
                    return false;
                }
            }

            return true;
        }

        public static bool HasParameter(this Animator animator, int nameHash) {
            for (int i = 0; i < animator.parameters.Length; i++) {
                if (animator.parameters[i].nameHash == nameHash) {
                    return true;
                }
            }

            return false;
        }

        public static bool HasParameter(this Animator animator, string name) {
            for (int i = 0; i < animator.parameters.Length; i++) {
                if (animator.parameters[i].name == name) {
                    return true;
                }
            }

            return false;
        }

        public static object MemberwiseClone(this object obj) {
            if (obj == null || obj.GetType().IsValueType)
                return obj;

            var type = obj.GetType();
            var result = Activator.CreateInstance(type);
            foreach (var field in type.GetCachedFields(BindingFlags.Instance | BindingFlags.Public)) {
                field.SetValue(result, field.GetValue(obj));
            }

            foreach (var property in type.GetCachedProperties(BindingFlags.Instance | BindingFlags.Public)) {
                if (property.GetIndexParameters().Length > 0 || !property.CanRead || !property.CanWrite)
                    continue;

                property.SetValue(result, property.GetValue(obj, null), null);
            }

            return result;
        }

        public static string Unzip(this string str) {
            var bytes = Convert.FromBase64String(str);
            var unzippedBytes = bytes.Unzip();
            return Encoding.UTF8.GetString(unzippedBytes);
        }

        public static byte[] Unzip(this byte[] bytes) {
            var dataLength = BitConverter.ToInt32(bytes, 0);
            var buffer = new byte[dataLength];
            using (var memoryStream = new MemoryStream()) {
                memoryStream.Write(bytes, 4, bytes.Length - 4);
                memoryStream.Position = 0;

                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }
            }

            return buffer;
        }

        public static string Zip(this string str) {
            var bytes = Encoding.UTF8.GetBytes(str);
            var zippedBytes = bytes.Zip();
            return Convert.ToBase64String(zippedBytes);
        }

        public static byte[] Zip(this byte[] bytes) {
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                gZipStream.Write(bytes, 0, bytes.Length);
            }

            memoryStream.Position = 0;

            var buffer = new byte[memoryStream.Length + 4];
            if (memoryStream.TryGetBuffer(out var arraySeg)) {
                Buffer.BlockCopy(arraySeg.Array, arraySeg.Offset, buffer, 4, arraySeg.Count);
                Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, buffer, 0, 4);
            }
            else {
                Buffer.BlockCopy(memoryStream.ToArray(), 0, buffer, 4, (int) memoryStream.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, buffer, 0, 4);
            }

            return buffer;
        }
    }
}