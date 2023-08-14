using System;
using System.Text.RegularExpressions;

#nullable enable

namespace jwelloneEditor
{
    public static class TypeNameConvert
    {
        interface ITypeNameConvert
        {
            bool TryConvert(string typeName, out string result);
        }

        readonly static ITypeNameConvert[] _convertTable = new ITypeNameConvert[]
        {
            new CollectionTypeConvert(),
            new VoidTypeConvert(),
            new BoolTypeConvert(),
            new ByteTypeConvert(),
            new SByteTypeConvert(),
            new IntTypeConvert(),
            new UIntTypeConvert(),
            new LongTypeConvert(),
            new ULongTypeConvert(),
            new SingleTypeConvert(),
            new DoubleTypeConvert(),
            new StringTypeConvert(),
        };

        public static string Convert(Type type)
        {
            return Convert(type.ToString());
        }

        public static string Convert(string typeName)
        {
            typeName = typeName.Replace("&", "").Replace("+", ".");
            foreach (var convert in _convertTable)
            {
                if (convert.TryConvert(typeName, out var result))
                {
                    return result;
                }
            }

            return typeName;
        }

        static bool TryReplace(string typeName, string keyword, string replaceString, out string result)
        {
            if (typeName.Contains(keyword))
            {
                result = typeName.Replace(keyword, replaceString);
                return true;
            }

            result = string.Empty;
            return false;
        }

        static bool TryCollectionReplace(string typeName, out string result)
        {
            var index = typeName.IndexOf('`');
            if (index < 0)
            {
                result = string.Empty;
                return false;
            }

            var matches = Regex.Matches(typeName, @"(?<=\[).*?(?=\])");
            if (matches.Count == 0)
            {
                result = string.Empty;
                return false;
            }

            var split = matches[0].ToString().Split(',');
            var parameters = new string[split.Length];
            for (var i = 0; i < split.Length; ++i)
            {
                parameters[i] = Convert(split[i]);
            }

            result = $"{typeName.Substring(0, index)}<{string.Join(", ", parameters)}>";
            return true;
        }

        sealed class VoidTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Void", "void", out result);
            }
        }

        sealed class BoolTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Boolean", "bool", out result);
            }
        }

        sealed class ByteTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Byte", "byte", out result);
            }
        }

        sealed class SByteTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.SByte", "sbyte", out result);
            }
        }

        sealed class IntTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Int32", "int", out result);
            }
        }

        sealed class UIntTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.UInt32", "uint", out result);
            }
        }


        sealed class LongTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Int64", "long", out result);
            }
        }

        sealed class ULongTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.UInt64", "ulong", out result);
            }
        }

        sealed class SingleTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Single", "float", out result);
            }
        }

        sealed class DoubleTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.Double", "double", out result);
            }
        }

        sealed class StringTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryReplace(typeName, "System.String", "string", out result);
            }
        }

        sealed class CollectionTypeConvert : ITypeNameConvert
        {
            bool ITypeNameConvert.TryConvert(string typeName, out string result)
            {
                return TryCollectionReplace(typeName, out result);
            }
        }
    }
}