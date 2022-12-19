using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Transform[,] Tiles
    {
        get; private set;
    }

    public int TileAmountInRow
    {
        get; set;
    }

    void Start()
    {
        TileAmountInRow = 4;
        SetBoard(TileAmountInRow);
    }

    public void SetBoard(int tileAmountInRow)
    {
        TileAmountInRow = tileAmountInRow;
        float boardSize = transform.localScale.x;
        Util.InitialiseChild(transform);

        Transform tileCollectionObject = Util.AddEmptyGameObject("Tiles", transform);
        Tiles = SetTiles(boardSize, tileAmountInRow, tileCollectionObject);

        Manager.Resource.Instantiate("ActiveTiles", transform);
    }

    Transform[,] SetTiles(float boardSize, int tileAmountInRow, Transform parent)
    {
        Transform[,] tiles = new Transform[tileAmountInRow, tileAmountInRow];

        // ������ ����ɷ��� ��� 3X3�̿��� ��
        if (tileAmountInRow < 3) 
        {
            Debug.Log("���� : ������ ����ɷ��� Ÿ�� �� �ٿ� ��� 3���� Ÿ���� �����ؾ� �մϴ�.");
            Debug.Log($" ���� ������ �� : {tileAmountInRow}");
            return null;
        }

        // Ÿ�� ������ ���� Ÿ�� ũ�� ��� (padding = 20)
        float tileSize = (boardSize - (tileAmountInRow + 1) * 20) / tileAmountInRow; 

        float axisYUsedSize = 0;
        for (int yAxisIndex = tileAmountInRow - 1; yAxisIndex >= 0; yAxisIndex--)
        {
            axisYUsedSize += 20;
            axisYUsedSize += tileSize / 2;
            float objectYPosition = (axisYUsedSize - (boardSize / 2)) / 100;

            float axisXUsedSize = 0;
            for (int xAxisIndex = 0; xAxisIndex < tileAmountInRow; xAxisIndex++)
            {
                axisXUsedSize += 20;
                axisXUsedSize += tileSize / 2;
                float objectXPosition = (axisXUsedSize - (boardSize / 2)) / 100;

                GameObject createdObject = Manager.Resource.Instantiate("Tile", parent);

                tiles[yAxisIndex, xAxisIndex] = createdObject.transform;

                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0);
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1);
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2;
        }

        return tiles;
    }
}
