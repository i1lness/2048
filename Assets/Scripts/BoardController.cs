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

    /* 게임보드 만드는 함수 (오브젝트 배치) */
    Transform MakeBoard()
    {
        Transform tileCollection; // 타일을 모아놓은 오브젝트가 될 예정

        Transform parent = transform.parent;

        if (parent != null) 
        {
            if (parent.Find("Tiles") == null) // 부모의 자식 중 Tiles 가 없을 경우 생성 후 부모와 연결 및 변수로 저장
            {
                tileCollection = new GameObject("Tiles").transform;
                tileCollection.transform.parent = parent;
            }
            else // 부모의 자식 중 Tiles 가 있을 경우 변수로 저장
            {
                tileCollection = parent.Find("Tiles");
            }
        }
        else // Board에게 부모가 없을 경우 그냥 새로 Tiles 오브젝트 생성 후 변수로 저장
        {
            tileCollection = new GameObject("Tiles").transform;
        }

        return tileCollection;
    }

    /* 게임보드 안에 배치할 타일 생성 후 배치하는 함수 */
    void SetTiles(float boardSize, int tileAmountInRow, Transform tileCollection)
    {
        if (tileAmountInRow < 3) // 게임이 진행될려면 적어도 3X3이여야 함
        {
            Debug.Log("게임이 진행될려면 타일 한 줄에 적어도 3개의 타일이 존재해야 합니다.");
            Debug.Log($" 현재 설정된 값 : {tileAmountInRow}");
            return;
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

                GameObject createdObject = Manager.Resource.Instantiate("Tile", tileCollection);
                createdObject.transform.position = new Vector3(objectXPosition, objectYPosition, 0);
                createdObject.transform.localScale = new Vector3(tileSize, tileSize, 1);
                axisXUsedSize += tileSize / 2;
            }
            axisYUsedSize += tileSize / 2;
        }
    }
}
