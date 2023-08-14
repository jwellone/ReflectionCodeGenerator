// =================================================
// This is an automatically generated file.
// Unable to edit.
// Create by class TypeToTextConvert.
// =================================================
using System;
using System.Reflection;

#nullable enable

namespace jwellone.Samples
{
	public static partial class HogeReflection
	{
		static Type? _cacheType;
		static Type? _type => _cacheType ??= Type.GetType("jwellone.Samples.Hoge, jwellone.ReflectionCodeGenerator.Samples.Scripts, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
	
		public static void Set_memberIntValue(object self, int value)
		{
			var info = jwellone.ReflectionUtil.GetFieldInfo(_type, "_memberIntValue", BindingFlags.Instance|BindingFlags.NonPublic);
			info?.SetValue(self, value);
		}
		public static int Get_memberIntValue(object self)
		{
			var info = jwellone.ReflectionUtil.GetFieldInfo(_type, "_memberIntValue", BindingFlags.Instance|BindingFlags.NonPublic);
			if(info == null)
			{
				 return default!;
			}
			return (int)info!.GetValue(self);
		}
		public static void Set_staticIntValue(int value)
		{
			var info = jwellone.ReflectionUtil.GetFieldInfo(_type, "_staticIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			info?.SetValue(null, value);
		}
		public static int Get_staticIntValue()
		{
			var info = jwellone.ReflectionUtil.GetFieldInfo(_type, "_staticIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			if(info == null)
			{
				 return default!;
			}
			return (int)info!.GetValue(null);
		}
	
		public static void Log(object self)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "Log", BindingFlags.Instance|BindingFlags.NonPublic, new Type[0] { });
			if (methodInfo == null)
			{
				return;
			}
			var p = new object[] { };
			methodInfo!.Invoke(self, p);
		}
		public static void Log(object self, int value)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "Log", BindingFlags.Instance|BindingFlags.NonPublic, new Type[1] { typeof(int) });
			if (methodInfo == null)
			{
				return;
			}
			var p = new object[] { value };
			methodInfo!.Invoke(self, p);
		}
		public static void Finalize(object self)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "Finalize", BindingFlags.Instance|BindingFlags.NonPublic, new Type[0] { });
			if (methodInfo == null)
			{
				return;
			}
			var p = new object[] { };
			methodInfo!.Invoke(self, p);
		}
		public static System.Object MemberwiseClone(object self)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "MemberwiseClone", BindingFlags.Instance|BindingFlags.NonPublic, new Type[0] { });
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (System.Object)methodInfo!.Invoke(self, p);
			return returnValue;
		}
		public static byte GetByteValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetByteValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (byte)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static sbyte GetSByteValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetSByteValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (sbyte)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static int GetIntValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (int)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static uint GetUIntValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetUIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (uint)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static long GetLongValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetLongValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (long)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static ulong GetULongValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetULongValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (ulong)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static float GetFloatValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetFloatValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (float)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static double GetDoubleValue()
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetDoubleValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				return default!;
			}
			var p = new object[] { };
			var returnValue = (double)methodInfo!.Invoke(null, p);
			return returnValue;
		}
		public static void GetOutIntValue(out int result)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetOutIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				result = default!;
				return;
			}
			var p = new object[] { null! };
			methodInfo!.Invoke(null, p);
			result = (int)p[0];
		}
		public static void GetRefIntValue(ref int result)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "GetRefIntValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				result = default!;
				return;
			}
			var p = new object[] { result };
			methodInfo!.Invoke(null, p);
			result = (int)p[0];
		}
		public static bool TryGetValue(out int value)
		{
			var methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, "TryGetValue", BindingFlags.Static|BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				value = default!;
				return default!;
			}
			var p = new object[] { null! };
			var returnValue = (bool)methodInfo!.Invoke(null, p);
			value = (int)p[0];
			return returnValue;
		}
	}
}