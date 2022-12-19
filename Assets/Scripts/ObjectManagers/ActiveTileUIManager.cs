using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveTileUIManager : MonoBehaviour
{
    Transform _activeTile;

    void Start()
    {

        Canvas canvas = transform.parent.GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("Sub Camera").GetComponent<Camera>();
        canvas.sortingLayerName = "ActionTileScoreUI";

        _activeTile = transform.parent.parent;

        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate -= UpdateUIInfo;
        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate += UpdateUIInfo;

        // �ѹ� �������� ������ ����ȭ��Ų�� (����Ƽ Ư���� ù ����ȭ�� �۵��� ����)
        UpdateUIInfo(_activeTile.GetComponent<ActiveTileManager>()._tileScore); 
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
