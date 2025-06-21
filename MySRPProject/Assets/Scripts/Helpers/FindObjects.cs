using UnityEngine;

public static class FindObjects
{
    public static Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;

            // Optional: search recursively
            Transform result = FindChildWithTag(child, tag);
            if (result != null)
                return result;
        }

        return null;
    }
}
