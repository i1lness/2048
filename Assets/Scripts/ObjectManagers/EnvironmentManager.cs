using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Transform _board;

    public GameObject _gameOverText;

    void Start()
    {
        if (transform.Find("Board") == null) // _board Á¤ÀÇ
        {
            _board = Manager.Resource.Instantiate("Board", transform).transform;
        }
        else
        {
            _board = transform.Find("Board");
        }

        if (_gameOverText == null)
            _gameOverText = transform.Find("GameOverText").gameObject;

        ActiveTilesManager.NoTileSpaceLeft -= EnableGameOverUI;
        ActiveTilesManager.NoTileSpaceLeft += EnableGameOverUI;

        MenuUIManager.ResetBoardAction -= DisableGameOverUI;
        MenuUIManager.ResetBoardAction += DisableGameOverUI;

        DisableGameOverUI();
    }

    void EnableGameOverUI()
    {
        _gameOverText.SetActive(true);
    }

    void DisableGameOverUI()
    {
        _gameOverText.SetActive(false);
    }
}
