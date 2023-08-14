using UnityEngine;

namespace jwellone.Samples
{
    public class SampleScene : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 128, 32), "Log1"))
            {
                Debug.Log($"staticIntValue => {HogeReflection.Get_staticIntValue()}");
                HogeReflection.Set_staticIntValue(123);
                Debug.Log($"staticIntValue => {HogeReflection.Get_staticIntValue()}");

                Debug.Log($"GetByteValue => {HogeReflection.GetByteValue()}");
                Debug.Log($"GetIntValue => {HogeReflection.GetIntValue()}");
                Debug.Log($"GetDoubleValue => {HogeReflection.GetDoubleValue()}");

                var hoge = new Hoge();
                HogeReflection.Log(hoge);
                HogeReflection.Log(hoge, 111);
            }

            if (GUI.Button(new Rect(0, 32, 128, 32), "Log2"))
            {
                Debug.Log($"staticIntValue => {ReflectionUtil.GetField<Hoge>("_staticIntValue")}");
                ReflectionUtil.SetField<Hoge>("_staticIntValue", 111);
                Debug.Log($"staticIntValue => {ReflectionUtil.GetField<Hoge>("_staticIntValue")}");

                Debug.Log($"GetByteValue => {ReflectionUtil.GetMethod<Hoge>("GetByteValue")}");
                Debug.Log($"GetIntValue => {ReflectionUtil.GetMethod<Hoge>("GetIntValue")}");
                Debug.Log($"GetDoubleValue => {ReflectionUtil.GetMethod<Hoge>("GetDoubleValue")}");

                var hoge = new Hoge();
                ReflectionUtil.Method(hoge, "Log");
                ReflectionUtil.Method(hoge, "Log", 256);
            }
        }
    }
}