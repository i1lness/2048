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

        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate -= UpdateUIInfo; // Ÿ���� ������ ActiveTileManager���� ������Ʈ������ delegate�� �������� UI�� ������Ʈ �ǵ��� �Ѵ�
        _activeTile.GetComponent<ActiveTileManager>().TileUIInfoUpdate += UpdateUIInfo;

        UpdateUIInfo(_activeTile.GetComponent<ActiveTileManager>()._tileScore); // �ѹ� �������� ������ ����ȭ��Ų��
    }

    /* Ÿ�ϰ� �����ؽ�Ʈ(UI)��ġ �����ϴ� �Լ� */
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

    /* UI �ؽ�Ʈ ����� ���� ������Ʈ�ϴ� �Լ� */
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
