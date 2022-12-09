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

    public void InitialiseController(Transform parent)
    {
        ConnectedUI = Manager.Resource.Instantiate("TileScoreUI", parent).transform;
        /* To do
         * 3. UI�� ������ ������Ʈ �Լ� �����
         */
    }

    void PositionUpdate()
    {
        Vector3 a = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 b = GameObject.Find("Sub Camera").GetComponent<Camera>().ViewportToWorldPoint(a);
        ConnectedUI.position = b;
    }

    void Update()
    {
        TileScore = transform.GetComponent<ActiveTileInfo>()._tileScore;
        ConnectedUI.GetComponent<TextMeshProUGUI>().text = TileScore.ToString();
        PositionUpdate();
    }
}
