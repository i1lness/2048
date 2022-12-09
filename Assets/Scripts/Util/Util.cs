
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

    public static Transform GetOrAddEmptyGameObject(string name, Transform parent = null)
    {
        Transform gameObject;

        if (parent != null)
        {
            if (parent.Find(name) == null)
            {
                gameObject = new GameObject(name).transform;
                gameObject.parent = parent;
            }
            else
            {
                gameObject = parent.Find(name);
            }
        }
        else
        {
            gameObject = new GameObject(name).transform;
        }

        return gameObject;
    }
}
