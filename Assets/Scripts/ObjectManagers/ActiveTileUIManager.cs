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

        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate -= UpdateUIInfo; // 타일의 정보가 ActiveTileManager에서 업데이트됨으로 delegate로 연동시켜 UI도 업데이트 되도록 한다
        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate += UpdateUIInfo;

        UpdateUIInfo(_activeTile.GetComponent<ActiveTileManager>()._tileScore); // 한번 수동으로 점수를 동기화시킨다
    }

    /* 타일과 점수텍스트(UI)위치 연동하는 함수 */
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

    /* UI 텍스트 내용과 색깔 업데이트하는 함수 */
    void UpdateUIInfo(int tileScore)
    {
        TextMeshProUGUI textComponent = transform.GetComponent<TextMeshProUGUI>();
        if (tileScore <= 32)
        {
            textComponent.color = new Color(0.22f, 0.22f, 0.2f, 0.75f);
        }
        else
        {
            textComponent.color = Color.white;
        }

        textComponent.text = tileScore.ToString();
    }
}
