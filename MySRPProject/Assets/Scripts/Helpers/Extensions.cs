using UnityEngine;

namespace Helpers
{
    public enum VectorCoordinate
    {
        X,
        Y,
    }

    public static class Extensions
    {
        public static bool IsNearZero(this Vector2 vector2, bool useAbsolute = false, VectorCoordinate coordinate = VectorCoordinate.X)
        {
            if (useAbsolute)
            {
                vector2.x = Mathf.Abs(vector2.x);
                vector2.y = Mathf.Abs(vector2.y);
            }

            var result = coordinate == VectorCoordinate.X ? 
                vector2.x >= 0.0f && vector2.x < 0.001f : 
                vector2.y >= 0.0f && vector2.y < 0.001f;
            
            return result;
        }
    }
}