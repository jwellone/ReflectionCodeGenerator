using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

#nullable enable

namespace jwellone
{
    public static class ReflectionUtil
    {
        const BindingFlags STATIC_FLAGS = BindingFlags.Static | BindingFlags.NonPublic;
        const BindingFlags INSTANCE_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;
        readonly static Dictionary<string, FieldInfo> _fieldInfoDictionary = new Dictionary<string, FieldInfo>();
        readonly static Dictionary<string, PropertyInfo> _propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
        readonly static Dictionary<string, MethodInfo> _methodInfoDictionary = new Dictionary<string, MethodInfo>();

        public static void SetField<T>(string fieldName, object param)
        {
            var fieldInfo = GetFieldInfo(typeof(T), fieldName, STATIC_FLAGS);
            fieldInfo?.SetValue(null, param);
        }

        public static object? GetField<T>(string fieldName)
        {
            var fieldInfo = GetFieldInfo(typeof(T), fieldName, STATIC_FLAGS);
            return fieldInfo?.GetValue(null);
        }

        public static void SetField<T>(T target, string fieldName, object param)
        {
            var fieldInfo = GetFieldInfo(typeof(T), fieldName, INSTANCE_FLAGS);
            fieldInfo?.SetValue(target, param);
        }

        public static object? GetField<T>(T target, string fieldName)
        {
            var fieldInfo = GetFieldInfo(typeof(T), fieldName, INSTANCE_FLAGS);
            return fieldInfo?.GetValue(target);
        }

        public static object? GetProperty<T>(string propertyName)
        {
            var propertyInfo = GetPropertyInfo(typeof(T), propertyName, STATIC_FLAGS);
            return propertyInfo?.GetValue(null);
        }

        public static void SetProperty<T>(string propertyName, object param)
        {
            var propertyInfo = GetPropertyInfo(typeof(T), propertyName, STATIC_FLAGS);
            propertyInfo?.SetValue(null, param);
        }

        public static object? GetProperty<T>(T target, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(typeof(T), propertyName, INSTANCE_FLAGS);
            return propertyInfo?.GetValue(target);
        }

        public static void SetProperty<T>(T target, string propertyName, object param)
        {
            var propertyInfo = GetPropertyInfo(typeof(T), propertyName, INSTANCE_FLAGS);
            propertyInfo?.SetValue(target, param);
        }

        public static void Method<T>(string methodName, params object[] args)
        {
            var methodInfo = GetMethodInfo(typeof(T), methodName, STATIC_FLAGS, ArgsToTypes(args));
            methodInfo?.Invoke(null, args);
        }

        public static object? GetMethod<T>(string methodName, params object[] args)
        {
            var methodInfo = GetMethodInfo(typeof(T), methodName, STATIC_FLAGS, ArgsToTypes(args));
            return methodInfo?.Invoke(null, args);
        }

        public static void Method<T>(T target, string methodName, params object[] args)
        {
            var methodInfo = GetMethodInfo(typeof(T), methodName, INSTANCE_FLAGS, ArgsToTypes(args));
            methodInfo?.Invoke(target, args);
        }

        public static object? GetMethod<T>(T target, string methodName, params object[] args)
        {
            var methodInfo = GetMethodInfo(typeof(T), methodName, INSTANCE_FLAGS, ArgsToTypes(args));
            return methodInfo?.Invoke(target, args);
        }

        public static FieldInfo? GetFieldInfo(Type? target, string fieldName, BindingFlags bindingFlags)
        {
            if (target == null)
            {
                return null;
            }

            var key = $"{target.FullName}.{fieldName}";
            if (_fieldInfoDictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            var info = target.GetField(fieldName, bindingFlags);
            if (info != null)
            {
                _fieldInfoDictionary.Add(key, info);
            }

            return info;
        }

        public static PropertyInfo? GetPropertyInfo(Type? target, string propertyName, BindingFlags bindingFlags)
        {
            if (target == null)
            {
                return null;
            }

            var key = $"{target.FullName}.{propertyName}";
            if (_propertyInfoDictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            var info = target.GetProperty(propertyName, bindingFlags);
            if (info != null)
            {
                _propertyInfoDictionary.Add(key, info);
            }

            return info;
        }

        public static MethodInfo? GetMethodInfo(Type? target, string methodName, BindingFlags bindingFlags, Type[]? types = null)
        {
            if (target == null)
            {
                return null;
            }

            var key = $"{target.FullName}.{methodName}";
            if (types != null)
            {
                foreach (var t in types)
                {
                    key += t.ToString();
                }
            }

            if (_methodInfoDictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            UnityEngine.Debug.Log($"{target.GetType()}.{methodName} {types == null}");
            var info = types == null ? target.GetMethod(methodName, bindingFlags) : target.GetMethod(methodName, bindingFlags, null, types, null);

            Assert.IsNotNull(info, $"Failed to get med information for {target.Name}.{methodName}.");

            if (info != null)
            {
                _methodInfoDictionary.Add(key, info);
            }

            return info;
        }

        static Type[]? ArgsToTypes(object[] args)
        {
            var types = new Type[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                types[i] = args[i].GetType();
            }

            return types;
        }
    }
}