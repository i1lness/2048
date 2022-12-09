using UnityEngine;

public static class Extension
{
    public static Transform InitialiseChild(this Transform parent)
    {
        return Util.InitialiseChild(parent);
    }
}
