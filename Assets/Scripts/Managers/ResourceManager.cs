using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    /* ���� ������Ʈ ȣ�� �Լ� */
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    /* ���� ������Ʈ ���� �Լ� */
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject createdObject = Object.Instantiate(prefab, parent);

        int index = createdObject.name.IndexOf("(Clone)"); // ���� �� ������Ʈ �̸��� (Clone)�κ� ����
        if (index > 0)
        {
            createdObject.name = createdObject.name.Substring(0, index);
        }

        return createdObject;
    }

    /* ���� ������Ʈ �ı� �Լ� */
    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }
}
