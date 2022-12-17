using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTextManager : MonoBehaviour
{
    void Start()
    {
        ActiveTilesManager.NoTileSpaceLeft -= EnableMenu;
        ActiveTilesManager.NoTileSpaceLeft += EnableMenu;

        MenuUIManager.ResetBoardAction -= DisableMenu;
        MenuUIManager.ResetBoardAction += DisableMenu;

        DisableMenu();
    }

    void Update()
    {

    }

    void EnableMenu()
    {
        gameObject.SetActive(true);
    }

    void DisableMenu()
    {
        gameObject.SetActive(false);
    }
}
