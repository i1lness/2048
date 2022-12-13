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

    /* 보드 세팅 후 타일들을 반환하는 함수*/
    Transform[,] SetBoard(float boardSize, int tileAmountInRow)
    {
        Transform[,] tiles;

        Util.InitialiseChild(transform);

        Transform tileCollectionObject = Util.AddEmptyGameObject("Tiles", transform).InitialiseChild();
        tiles = SetTiles(boardSize, tileAmountInRow, tileCollectionObject); // 만들어진 타일들을 저장

        Transform actionTileCollectionObject = Util.AddEmptyGameObject("ActionTiles", transform).InitialiseChild();
        actionTileCollectionObject.AddComponent<ActiveTilesManager>();

        return tiles; // 저장한 타일들 반환
    }

    /* 게임보드 안에 기본적으로 배치할 타일 생성 및 배치 후 만들어진 타일들을 반환하는 함수 */
    Transform[,] SetTiles(float boardSize, int tileAmountInRow, Transform parent)
    {
        Transform[,] tiles = new Transform[tileAmountInRow, tileAmountInRow];

        if (tileAmountInRow < 3) // 게임이 진행될려면 적어도 3X3이여야 함
        {
            Debug.Log("에러 : 게임이 진행될려면 타일 한 줄에 적어도 3개의 타일이 존재해야 합니다.");
            Debug.Log($" 현재 설정된 값 : {tileAmountInRow}");
            return null;
        }

        float tileSize = (boardSize - (tileAmountInRow + 1) * 20) / tileAmountInRow; // 타일 갯수에 따른 타일 크기 계산 (padding = 20)

        float axisYUsedSize = 0;
        for (int yAxisIndex = tileAmountInRow - 1; yAxisIndex >= 0; yAxisIndex--) // 배치되어야 할 위치와 타일의 크기 계산 (#1)
        {
            axisYUsedSize += 20;
            axisYUsedSize += tileSize / 2;
            float objectYPosition = (axisYUsedSize - (boardSize / 2)) / 100;

            float axisXUsedSize = 0;
            for (int xAxisIndex = 0; xAxisIndex < tileAmountInRow; xAxisIndex++)
            {
                axisXUsedSize += 20;
                axisXUsedSize += tileSize / 2;
                float objectXPosition = (axisXUsedSize - (boardSize / 2)) / 100; // (#1) 여기까지가 계산 로직

                GameObject createdObject = Manager.Resource.Instantiate("Tile", parent);

                tiles[yAxisIndex, xAxisIndex] = createdObject.transform; // 생성된 오브젝트를 함수값으로 넘기기 위해 저장

                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0); // 생성된 타일에 계산된 위치값 적용
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1); // 생성된 타일에 계산된 크기값 적용
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2; // (#1) 이것도 계산 로직
        }

        return tiles;
    }
}
