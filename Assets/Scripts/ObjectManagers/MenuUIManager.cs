using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    bool isCanvasActive;

    int tileAmountInRow = 4;

    [SerializeField]
    Transform Environment;

    public void MakeBoard()
    {
        Environment.GetComponent<EnvironmentManager>()._board.GetComponent<BoardManager>().SetBoard(tileAmountInRow);
        DisableMenu();
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

        DisableMenu();
    }

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
