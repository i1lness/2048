using UnityEngine;

public class ScreenFix : MonoBehaviour
{
    private void Start()
    {
        SetResolution();
    }

    /* ȭ�� ���� �����ϴ� �Լ� */
    public void SetResolution()
    {
        int setWidth = 1920; // ���ϴ� ���� �ʺ�
        int setHeight = 1080; // ���ϴ� ���� ����

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // ��üȭ������ �����

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ���ϴ� �������� ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���� ������ �ʺ� ��� (����)
            transform.GetComponent<Camera>().rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���� ������ ���� ��� (����)
            transform.GetComponent<Camera>().rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}
