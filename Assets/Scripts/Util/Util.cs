
using UnityEngine;

public class Util
{
    public static Transform InitialiseChild(Transform parent)
    {
        for (int index = 0; index < parent.childCount; index++)
        {
            Transform childObject = parent.GetChild(index);
            Manager.Resource.Destroy(childObject.gameObject);
        }

        return parent;
    }

    public static Transform AddEmptyGameObject(string name, Transform parent = null)
    {
        Transform gameObject = new GameObject(name).transform;

        if (parent != null)
        {
            gameObject.parent = parent;
        }

        return gameObject;
    }
}
