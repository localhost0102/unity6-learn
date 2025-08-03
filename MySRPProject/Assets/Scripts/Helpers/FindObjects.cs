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
    
    public static Transform FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform result = FindChildByName(child, childName);
            if (result != null)
                return result;
        }
        return null;
    }

}
