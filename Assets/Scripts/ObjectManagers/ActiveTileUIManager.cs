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
        _activeTile.GetComponent<ActiveTileInfo>().TileUIInfoUpdate -= UpdateUIInfo;
        _activeTile.GetComponent<ActiveTileInfo>().TileUIInfoUpdate += UpdateUIInfo;
        _activeTile.GetComponent<ActiveTileInfo>().ChangeTileColorByTileScore();
    }


    void UIPositionUpdate()
    {
        Vector3 viewpoint = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToViewportPoint(_activeTile.position);
        Vector3 worldpoint = GameObject.Find("Sub Camera").GetComponent<Camera>().ViewportToWorldPoint(viewpoint);
        transform.position = worldpoint;
    }

    void LateUpdate()
    {
        UIPositionUpdate();
    }

    void UpdateUIInfo(int tileScore)
    {
        if (tileScore <= 16)
        {
            transform.GetComponent<TextMeshProUGUI>().color = new Color(0.22f, 0.22f, 0.2f, 0.75f);
        }
        else
        {
            transform.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        transform.GetComponent<TextMeshProUGUI>().text = tileScore.ToString();
    }
}
