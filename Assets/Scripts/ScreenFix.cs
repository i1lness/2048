using UnityEngine;

public class ScreenFix : MonoBehaviour
{
    private void Start()
    {
        SetResolution();
    }

    /* 화면 비율 조절하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1920; // 원하는 최종 너비
        int setHeight = 1080; // 원하는 최종 높이

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // 전체화면으로 만들기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 원하는 비율보다 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로 적용할 너비 계산 (비율)
            transform.GetComponent<Camera>().rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로 적용할 높이 계산 (비율)
            transform.GetComponent<Camera>().rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
