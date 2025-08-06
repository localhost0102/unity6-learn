using System;

namespace Helpers
{
    public class TTypeConvert
    {
        public static bool ConvertToBool<T>(T value)
        {
            if (value is bool b)
                return b;
    
            throw new InvalidCastException($"Cannot convert {typeof(T)} to bool.");
        }
    }
}