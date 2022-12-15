using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    bool isCanvasActive;

    int tileAmountInRow = 4;

    [SerializeField]
    Transform Environment;

    public void MakeBoard()
    {
        Environment.GetComponent<EnvironmentInfo>().MakeBoard(tileAmountInRow);
    }

    public void ExitToScreen()
    {
        Application.Quit();
    }

    void Start()
    {
        isCanvasActive = gameObject.activeInHierarchy;
        InputManager.escClickAction -= UISwitch;
        InputManager.escClickAction += UISwitch;
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
