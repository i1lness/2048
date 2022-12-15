using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveTileUIManager : MonoBehaviour
{
    Transform _activeTile;

    void Start()
    {
        _activeTile = transform.parent.parent;
    }


    void UIPositionUpdate()
    {
        Vector3 viewpoint = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToViewportPoint(_activeTile.position);
        Vector3 worldpoint = GameObject.Find("Sub Camera").GetComponent<Camera>().ViewportToWorldPoint(viewpoint);
        transform.position = worldpoint;
    }

    void LateUpdate()
    {
        int tileScore = _activeTile.GetComponent<ActiveTileInfo>()._tileScore;
        transform.GetComponent<TextMeshProUGUI>().text = tileScore.ToString();
        UIPositionUpdate();
    }
}
