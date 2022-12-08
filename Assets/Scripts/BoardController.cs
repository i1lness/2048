using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private int _tileAmountInRow = 4;

    private float _boardSize;

    void Start()
    {
        _boardSize = transform.localScale.x;
        Transform tileCollection = MakeBoard();
        SetTiles(_boardSize, _tileAmountInRow, tileCollection);
    }

    /* ���Ӻ��� ����� �Լ� (������Ʈ ��ġ) */
    Transform MakeBoard()
    {
        Transform tileCollection; // Ÿ���� ��Ƴ��� ������Ʈ�� �� ����

        Transform parent = transform.parent;

        if (parent != null) 
        {
            if (parent.Find("Tiles") == null) // �θ��� �ڽ� �� Tiles �� ���� ��� ���� �� �θ�� ���� �� ������ ����
            {
                tileCollection = new GameObject("Tiles").transform;
                tileCollection.transform.parent = parent;
            }
            else // �θ��� �ڽ� �� Tiles �� ���� ��� ������ ����
            {
                tileCollection = parent.Find("Tiles");
            }
        }
        else // Board���� �θ� ���� ��� �׳� ���� Tiles ������Ʈ ���� �� ������ ����
        {
            tileCollection = new GameObject("Tiles").transform;
        }

        return tileCollection;
    }

    /* ���Ӻ��� �ȿ� ��ġ�� Ÿ�� ���� �� ��ġ�ϴ� �Լ� */
    void SetTiles(float boardSize, int tileAmountInRow, Transform tileCollection)
    {
        if (tileAmountInRow < 3) // ������ ����ɷ��� ��� 3X3�̿��� ��
        {
            Debug.Log("������ ����ɷ��� Ÿ�� �� �ٿ� ��� 3���� Ÿ���� �����ؾ� �մϴ�.");
            Debug.Log($" ���� ������ �� : {tileAmountInRow}");
            return;
        }

        float tileSize = (boardSize - (tileAmountInRow + 1) * 20) / tileAmountInRow; // Ÿ�� ������ ���� Ÿ�� ũ�� ��� (padding = 20)

        float axisYUsedSize = 0;
        for (int index = tileAmountInRow; index > 0; index--) // ��ġ�Ǿ�� �� ��ġ�� Ÿ���� ũ�� ��� �� Ÿ�� ������Ʈ ���� �� ��ġ���� ũ�Ⱚ ����
        {
            axisYUsedSize += 20;
            axisYUsedSize += tileSize / 2;
            float objectYPosition = (axisYUsedSize - (boardSize / 2)) / 100;

            float axisXUsedSize = 0;
            for (int jndex = tileAmountInRow; jndex > 0; jndex--)
            {
                axisXUsedSize += 20;
                axisXUsedSize += tileSize / 2;
                float objectXPosition = (axisXUsedSize - (boardSize / 2)) / 100;

                GameObject createdObject = Manager.Resource.Instantiate("Tile", tileCollection);
                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0);
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1);
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2;
        }
    }
}
