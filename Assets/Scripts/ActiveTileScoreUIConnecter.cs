using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveTileScoreUIConnecter : MonoBehaviour
{
    public Transform ConnectedUI
    {
        get; private set;
    }

    public int TileScore
    {
        get; private set;
    }

    public void MakeAndConnectScoreUI(Transform parent)
    {
        ConnectedUI = Manager.Resource.Instantiate("TileScoreUI", parent).transform;
    }

    void UIPositionUpdate()
    {
        Vector3 viewpoint = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToViewportPoint(transform.position);
        Vector3 worldpoint = GameObject.Find("Sub Camera").GetComponent<Camera>().ViewportToWorldPoint(viewpoint);
        ConnectedUI.position = worldpoint;
    }

    void LateUpdate()
    {
        TileScore = transform.GetComponent<ActiveTileInfo>()._tileScore;
        ConnectedUI.GetComponent<TextMeshProUGUI>().text = TileScore.ToString();
        UIPositionUpdate();
    }
}
