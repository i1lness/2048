using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    bool isCanvasActive;

    void Start()
    {
        isCanvasActive = gameObject.activeInHierarchy;
        Manager.Input.escClickAction += UISwitch;
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
