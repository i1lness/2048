using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private float _boardSize;

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
        _boardSize = transform.localScale.x;
        TileAmountInRow = 4;
        Tiles = SetBoard(_boardSize, TileAmountInRow);
    }

    /* ���� ���� �� Ÿ�ϵ��� ��ȯ�ϴ� �Լ�*/
    Transform[,] SetBoard(float boardSize, int tileAmountInRow)
    {
        Transform[,] tiles;

        Util.InitialiseChild(transform);

        Transform tileCollectionObject = Util.GetOrAddEmptyGameObject("Tiles", transform).InitialiseChild();
        tiles = SetTiles(boardSize, tileAmountInRow, tileCollectionObject); // ������� Ÿ�ϵ��� ����

        Transform actionTileCollectionObject = Util.GetOrAddEmptyGameObject("ActionTiles", transform).InitialiseChild();
        ActiveTilesManager activeTilesManager = actionTileCollectionObject.AddComponent<ActiveTilesManager>();
        activeTilesManager._board = transform.GetComponent<BoardManager>(); // ActiveTilesManager�� board �Ӽ��� �ѱ�� ���ؼ� BoardManager ������Ʈ�� ����

        return tiles; // ������ Ÿ�ϵ� ��ȯ
    }

    /* ���Ӻ��� �ȿ� �⺻������ ��ġ�� Ÿ�� ���� �� ��ġ �� ������� Ÿ�ϵ��� ��ȯ�ϴ� �Լ� */
    Transform[,] SetTiles(float boardSize, int tileAmountInRow, Transform parent)
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
        for (int yAxisIndex = tileAmountInRow - 1; yAxisIndex >= 0; yAxisIndex--) // ��ġ�Ǿ�� �� ��ġ�� Ÿ���� ũ�� ��� (#1)
        {
            axisYUsedSize += 20;
            axisYUsedSize += tileSize / 2;
            float objectYPosition = (axisYUsedSize - (boardSize / 2)) / 100;

            float axisXUsedSize = 0;
            for (int xAxisIndex = 0; xAxisIndex < tileAmountInRow; xAxisIndex++)
            {
                axisXUsedSize += 20;
                axisXUsedSize += tileSize / 2;
                float objectXPosition = (axisXUsedSize - (boardSize / 2)) / 100; // (#1) ��������� ��� ����

                GameObject createdObject = Manager.Resource.Instantiate("Tile", parent);

                tiles[yAxisIndex, xAxisIndex] = createdObject.transform; // ������ ������Ʈ�� �Լ������� �ѱ�� ���� ����

                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0); // ������ Ÿ�Ͽ� ���� ��ġ�� ����
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1); // ������ Ÿ�Ͽ� ���� ũ�Ⱚ ����
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2; // (#1) �̰͵� ��� ����
        }

        return tiles;
    }
}
