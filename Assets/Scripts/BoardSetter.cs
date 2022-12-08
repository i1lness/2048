using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardSetter : MonoBehaviour
{
    private Transform _tileCollectionObject;
    private TilesInfo _tilesInfo = new TilesInfo();

    void Start()
    {
        SetBoardSize(transform.localScale.x);
        SetTileCountInRow(5);
        SetBoard();
    }

    public void SetBoardSize(float amount)
    {
        _tilesInfo.boardSize = amount;
    }

    public void SetTileCountInRow(int amount)
    {
        _tilesInfo.tileAmountInRow = amount;
    }

    public void SetBoard()
    {
        _tileCollectionObject = MakeTileCollection(gameObject.transform);
        _tilesInfo.activeTilesCollection = MakeActiveTileCollection(gameObject.transform);

        _tilesInfo.activeTilesCollection.AddComponent<TilesManager>()._tilesInfo = _tilesInfo;

        _tilesInfo.tiles = SetTiles(_tilesInfo.boardSize, _tilesInfo.tileAmountInRow, _tileCollectionObject);
    }

    Transform MakeTileCollection(Transform board)
    {
        Transform gameObject;

        if (board != null) 
        {
            if (board.Find("Tiles") == null) // Board�� �ڽ� �� Tiles ������Ʈ�� ���� ��� ���� �� �θ�� ����
            {
                gameObject = new GameObject("Tiles").transform;
                gameObject.parent = board;
            }
            else // Board�� �ڽ� �� Tiles ������Ʈ�� ���� ��� ã�� ����
            {
                gameObject = board.Find("Tiles");
            }
        }
        else // Board�� ���� ��� ���� �θ� ���� Tiles ������Ʈ ����
        {
            gameObject = new GameObject("Tiles").transform;
        }

        return gameObject; // ����ų� ã�� Tile ��ü �ּҰ� ��ȯ
    }

    Transform MakeActiveTileCollection(Transform board)
    {
        Transform gameObject;

        if (board != null)
        {
            if (board.Find("ActiveTiles") == null) // Board�� �ڽ� �� ActiveTiles ������Ʈ�� ���� ��� ���� �� �θ�� ����
            {
                gameObject = new GameObject("ActiveTiles").transform;
                gameObject.parent = board;
            }
            else // Board�� �ڽ� �� ActiveTiles ������Ʈ�� ���� ��� ã�� ����
            {
                gameObject = board.Find("ActiveTiles");
            }
        }
        else // Board�� ���� ��� ���� �θ� ���� ActiveTiles ������Ʈ ����
        {
            gameObject = new GameObject("ActiveTiles").transform;
        }

        return gameObject;
    }

    /* ���Ӻ��� �ȿ� �⺻������ ��ġ�� Ÿ�� ���� �� ��ġ �� Ÿ�ϵ��� ��ȯ�ϴ� �Լ� */
    Transform[,] SetTiles(float boardSize, int tileAmountInRow, Transform parent = null)
    {
        Transform[,] tiles = new Transform[tileAmountInRow, tileAmountInRow];

        if (tileAmountInRow < 3) // ������ ����ɷ��� ��� 3X3�̿��� ��
        {
            Debug.Log("���� : ������ ����ɷ��� Ÿ�� �� �ٿ� ��� 3���� Ÿ���� �����ؾ� �մϴ�.");
            Debug.Log($" ���� ������ �� : {tileAmountInRow}");
            return null;
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

                GameObject createdObject = Manager.Resource.Instantiate("Tile", parent);

                tiles[index - 1, tileAmountInRow - jndex] = createdObject.transform;

                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0);
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1);
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2;
        }

        return tiles;
    }
}
