using System.Resources;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_instance;
    InputManager _inputManager = new InputManager();
    ResourceManager _resource = new ResourceManager();

    static Manager Instance { get { Init(); return s_instance; } } // 만약 s_instance 존재X -> s_instance 정의

    public static InputManager Input { get { return Instance._inputManager; } }

    public static ResourceManager Resource { get { return Instance._resource; } }

    void Start()
    {
        Init(); // Manager 초기 설정
    }

    void Update()
    {
        Input.CheckInput(); // Input 확인 함수
    }

    /* Manager 초기생성 및 설정 함수 */
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

            s_instance = go.GetComponent<Manager>(); // 나중에 사용하기 위해 코드상에 오브젝트 저장
        }
    }
}
