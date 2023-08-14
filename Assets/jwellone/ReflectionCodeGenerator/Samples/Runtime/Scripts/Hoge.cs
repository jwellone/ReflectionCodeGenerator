using UnityEngine;

#nullable enable

namespace jwellone.Samples
{
    public class Hoge
    {
        private int _memberIntValue;
        private static int _staticIntValue;

        void Log()
        {
            Debug.Log($"Hoge.Log");
        }

        void Log(int value)
        {
            Debug.Log($"Hoge.Log({value})");
        }

        static byte GetByteValue()
        {
            return 0xf;
        }

        static sbyte GetSByteValue()
        {
            return 0xc;
        }

        static int GetIntValue()
        {
            return 10;
        }

        static uint GetUIntValue()
        {
            return 11;
        }

        static long GetLongValue()
        {
            return 100;
        }

        static ulong GetULongValue()
        {
            return 110;
        }

        static float GetFloatValue()
        {
            return 0.134f;
        }

        static double GetDoubleValue()
        {
            return 0.234;
        }

        static void GetOutIntValue(out int result)
        {
            result = 11;
        }

        static void GetRefIntValue(ref int result)
        {
            result = 111;
        }

        static bool TryGetValue(out int value)
        {
            value = 111111;
            return true;
        }
    }
}