using System;

namespace AI.Condition
{
    [Serializable]
    public class DataValue
    {
        public ValueType type;
        public float floatValue;
        public int intValue;
        public bool boolValue;
        public string stringValue;

        public T GetValue<T>()
        {
            if (typeof(T) == typeof(float))
                return (T)(object)floatValue;
            if (typeof(T) == typeof(int))
                return (T)(object)intValue;
            if (typeof(T) == typeof(bool))
                return (T)(object)boolValue;
            if (typeof(T) == typeof(string))
                return (T)(object)stringValue;
            throw new InvalidCastException($"Cannot convert DataValue to {typeof(T)}");
        }
    }
}