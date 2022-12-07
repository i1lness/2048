using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    /* 게임 오브젝트 호출 함수 */
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    /* 게임 오브젝트 생성 함수 */
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject createdObject = Object.Instantiate(prefab, parent);

        int index = createdObject.name.IndexOf("(Clone)"); // 생성 시 오브젝트 이름의 (Clone)부분 제거
        if (index > 0)
        {
            createdObject.name = createdObject.name.Substring(0, index);
        }

        return createdObject;
    }

    /* 게임 오브젝트 파괴 함수 */
    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }
}
