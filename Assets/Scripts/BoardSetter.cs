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
            if (board.Find("Tiles") == null) // Board의 자식 중 Tiles 오브젝트가 없을 경우 생성 후 부모와 연결
            {
                gameObject = new GameObject("Tiles").transform;
                gameObject.parent = board;
            }
            else // Board의 자식 중 Tiles 오브젝트가 있을 경우 찾고 저장
            {
                gameObject = board.Find("Tiles");
            }
        }
        else // Board가 없을 경우 따로 부모 없이 Tiles 오브젝트 생성
        {
            gameObject = new GameObject("Tiles").transform;
        }

        return gameObject; // 만들거나 찾은 Tile 객체 주소값 반환
    }

    Transform MakeActiveTileCollection(Transform board)
    {
        Transform gameObject;

        if (board != null)
        {
            if (board.Find("ActiveTiles") == null) // Board의 자식 중 ActiveTiles 오브젝트가 없을 경우 생성 후 부모와 연결
            {
                gameObject = new GameObject("ActiveTiles").transform;
                gameObject.parent = board;
            }
            else // Board의 자식 중 ActiveTiles 오브젝트가 있을 경우 찾고 저장
            {
                gameObject = board.Find("ActiveTiles");
            }
        }
        else // Board가 없을 경우 따로 부모 없이 ActiveTiles 오브젝트 생성
        {
            gameObject = new GameObject("ActiveTiles").transform;
        }

        return gameObject;
    }

    /* 게임보드 안에 기본적으로 배치할 타일 생성 및 배치 후 타일들을 반환하는 함수 */
    Transform[,] SetTiles(float boardSize, int tileAmountInRow, Transform parent = null)
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
        for (int index = tileAmountInRow; index > 0; index--) // 배치되어야 할 위치와 타일의 크기 계산 후 타일 오브젝트 생성 후 위치값과 크기값 적용
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
