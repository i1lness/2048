using System.Resources;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_instance;
    InputManager _inputManager = new InputManager();
    ResourceManager _resource = new ResourceManager();

    static Manager Instance { get { Init(); return s_instance; } } // ���� s_instance ����X -> s_instance ����

    public static InputManager Input { get { return Instance._inputManager; } }

    public static ResourceManager Resource { get { return Instance._resource; } }

    void Start()
    {
        Init(); // Manager �ʱ� ����
    }

    void Update()
    {
        Input.CheckInput(); // Input Ȯ�� �Լ�
    }

    /* Manager �ʱ���� �� ���� �Լ� */
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Manager");

            if (go == null)
            {
                go = new GameObject { name = "@Manager" };
            }
            if (go.GetComponent<Manager>() == null)
            {
                go.AddComponent<Manager>();
            }

            DontDestroyOnLoad(go);

            s_instance = go.GetComponent<Manager>(); // ���߿� ����ϱ� ���� �ڵ�� ������Ʈ ����
        }
    }
}
