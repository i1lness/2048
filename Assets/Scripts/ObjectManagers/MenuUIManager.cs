using System;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    bool isCanvasActive;

    int tileAmountInRow = 4;

    public static Action ResetBoardAction; 

    [SerializeField]
    Transform Environment;

    public void MakeBoard()
    {
        Environment.GetComponent<EnvironmentManager>()._board.GetComponent<BoardManager>().SetBoard(tileAmountInRow);
        if (ResetBoardAction != null)
            ResetBoardAction.Invoke();
    }

    public void ExitToScreen()
    {
        Application.Quit();
    }

    public void SetTileAmountInRow()
    {
        tileAmountInRow = (int)transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>().value;
    }

    void Start()
    {
        isCanvasActive = gameObject.activeInHierarchy;

        InputManager.escClickAction -= UISwitch;
        InputManager.escClickAction += UISwitch;

        ResetBoardAction -= DisableMenu;
        ResetBoardAction += DisableMenu;

        DisableMenu();
    }

    /* UI 활성상태 변환하는 함수 */
    void UISwitch()
    {
        if (isCanvasActive)
        {
            DisableMenu();
        }
        else
        {
            EnableMenu();
        }
    }

    void EnableMenu()
    {
        gameObject.SetActive(true);
        isCanvasActive = true;
    }

    void DisableMenu()
    {
        gameObject.SetActive(false);
        isCanvasActive = false;
    }
}
