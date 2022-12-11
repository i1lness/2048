using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTilesManager : MonoBehaviour
{
    public BoardManager _board;
    private Transform _scoreUICanvas;

    private Transform[,] _tiles;
    private int _tileAmountInRow;

    private Transform[,] _linkedActiveTile;
    private List<int> _linkAbleList = new List<int>(); // 자리가 비워있는 타일의 _linkedActiveTile의 index를 번호로 저장하는 곳

    void Start()
    {
        _tiles = _board.Tiles; // 객체 초기화 구문 (#1)
        _tileAmountInRow = _board.TileAmountInRow;

        _linkedActiveTile = new Transform[_tileAmountInRow, _tileAmountInRow];

        for (int index = _tileAmountInRow * _tileAmountInRow - 1; index >= 0; index--)
        {
            _linkAbleList.Add(index);
        } // (#1) 여기까지

        _scoreUICanvas = Manager.Resource.Instantiate("ScoreUICanvas", _board.transform).transform; // 타일 점수 표시할 UI를 모아둘 곳 생성 및 저장

        MakeActiveTile();
        MakeActiveTile();

        Manager.Input.moveInputAction += UpdateAllTilePosition;
    }

    Transform MakeActiveTile()
    {
        Transform activeTile = Manager.Resource.Instantiate("ActiveTile").transform;
        activeTile.parent = transform;

        int randomNumber = UnityEngine.Random.Range(0, _linkAbleList.Count);
        int positionIndex = _linkAbleList[randomNumber]; // 쓸 index(비어있는 타일 번호) 불러오기
        _linkAbleList.RemoveAt(randomNumber);

        int yAxisIndex = positionIndex / _tileAmountInRow;
        int xAxisIndex = positionIndex % _tileAmountInRow;

        _linkedActiveTile[yAxisIndex, xAxisIndex] = activeTile; // 배열로 activeTile를 저장

        activeTile.position = _tiles[yAxisIndex, xAxisIndex].position; // 만들어진 ActiveTile의 위치를 원래있던 타일틀과 맞추기
        activeTile.localScale = Vector3.zero;
        activeTile.GetComponent<ActiveTileInfo>()._settedScale = _tiles[yAxisIndex, xAxisIndex].localScale;

       activeTile.GetComponent<ActiveTileInfo>().InitialisePositionVariable(); // 움직인 타일의 위치를 변수로 다시 저장

        activeTile.GetComponent<SpriteRenderer>().color = Color.blue; // TEMP
        
        activeTile.GetComponent<ActiveTileScoreUIConnecter>().MakeAndConnectScoreUI(_scoreUICanvas);

        return activeTile;
    }

    void UpdateAllTilePosition(Define.MoveInputType moveInputType)
    {
        switch (moveInputType)
        {
            case Define.MoveInputType.Up:
                MoveAllTileUp();
                break; 
            case Define.MoveInputType.Down:
                MoveAllTileDown();
                break; 
            case Define.MoveInputType.Left:
                MoveAllTileLeft();
                break; 
            case Define.MoveInputType.Right:
                MoveAllTileRight();
                break;
        }
        MakeActiveTile();
    } 

    void MoveAllTileUp()
    {
        for (int xAxisIndex = 0; xAxisIndex < _tileAmountInRow; xAxisIndex++)
        {
            int mergedTileAmount = 0;

            for (int yAxisIndex = 0; yAxisIndex < _tileAmountInRow; yAxisIndex++)
            {
                if (_linkedActiveTile[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { Int32.MinValue, xAxisIndex };

                bool isMerged = false;
                for (int yAxisUpperIndex = yAxisIndex - 1; yAxisUpperIndex - mergedTileAmount >= 0; yAxisUpperIndex--)
                {
                    if (_linkedActiveTile[yAxisUpperIndex, xAxisIndex] == null)
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisUpperIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisUpperIndex, xAxisIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // 원래 그 자리에 있던 Active 타일의 UI부분(점수 표시)까지 삭제
                        Manager.Resource.Destroy(_linkedActiveTile[yAxisUpperIndex, xAxisIndex].gameObject);
                        isMerged = true;
                        mergedTileAmount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[0] == Int32.MinValue)
                    continue;

                _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]] = _linkedActiveTile[yAxisIndex, xAxisIndex];
                _linkedActiveTile[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileInfo>()._settedPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

                _linkAbleList.Add(_tileAmountInRow * yAxisIndex + xAxisIndex);
                _linkAbleList.Remove(_tileAmountInRow * arrivalPoint[0] + arrivalPoint[1]);
            }
        }
    }

    void MoveAllTileDown()
    {
        for (int xAxisIndex = 0; xAxisIndex < _tileAmountInRow; xAxisIndex++)
        {
            int mergedTileAmount = 0;

            for (int yAxisIndex = _tileAmountInRow - 1; yAxisIndex >= 0; yAxisIndex--)
            {
                if (_linkedActiveTile[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { Int32.MinValue, xAxisIndex };

                bool isMerged = false;
                for (int yAxisDownIndex = yAxisIndex + 1; yAxisDownIndex + mergedTileAmount < _tileAmountInRow; yAxisDownIndex++)
                {
                    if (_linkedActiveTile[yAxisDownIndex, xAxisIndex] == null)
                    {
                        arrivalPoint[0] = yAxisDownIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisDownIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[0] = yAxisDownIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisDownIndex, xAxisIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject);
                        Manager.Resource.Destroy(_linkedActiveTile[yAxisDownIndex, xAxisIndex].gameObject);
                        isMerged = true;
                        mergedTileAmount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[0] == Int32.MinValue)
                    continue;

                _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]] = _linkedActiveTile[yAxisIndex, xAxisIndex];
                _linkedActiveTile[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileInfo>()._settedPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

                _linkAbleList.Add(_tileAmountInRow * yAxisIndex + xAxisIndex);
                _linkAbleList.Remove(_tileAmountInRow * arrivalPoint[0] + arrivalPoint[1]);
            }
        }
    }

    void MoveAllTileLeft()
    {
        for (int yAxisIndex = 0; yAxisIndex < _tileAmountInRow; yAxisIndex++)
        {
            int mergedTileAmount = 0;

            for (int xAxisIndex = 0; xAxisIndex < _tileAmountInRow; xAxisIndex++)
            {
                if (_linkedActiveTile[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { yAxisIndex, Int32.MinValue };

                bool isMerged = false;
                for (int xAxisLeftIndex = xAxisIndex - 1; xAxisLeftIndex - mergedTileAmount >= 0; xAxisLeftIndex--)
                {
                    if (_linkedActiveTile[yAxisIndex, xAxisLeftIndex] == null)
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisIndex, xAxisLeftIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisLeftIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // 원래 그 자리에 있던 Active 타일의 UI부분(점수 표시)까지 삭제
                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisLeftIndex].gameObject);
                        isMerged = true;
                        mergedTileAmount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[1] == Int32.MinValue)
                    continue;

                _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]] = _linkedActiveTile[yAxisIndex, xAxisIndex];
                _linkedActiveTile[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileInfo>()._settedPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

                _linkAbleList.Add(_tileAmountInRow * yAxisIndex + xAxisIndex);
                _linkAbleList.Remove(_tileAmountInRow * arrivalPoint[0] + arrivalPoint[1]);
            }
        }
    }

    void MoveAllTileRight()
    {
        for (int yAxisIndex = 0; yAxisIndex < _tileAmountInRow; yAxisIndex++)
        {
            int mergedTileAmount = 0;

            for (int xAxisIndex = _tileAmountInRow - 1; xAxisIndex >= 0; xAxisIndex--)
            {
                if (_linkedActiveTile[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { yAxisIndex, Int32.MinValue };

                bool isMerged = false;
                for (int xAxisRightIndex = xAxisIndex + 1; xAxisRightIndex + mergedTileAmount < _tileAmountInRow; xAxisRightIndex++)
                {
                    if (_linkedActiveTile[yAxisIndex, xAxisRightIndex] == null)
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisIndex, xAxisRightIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisRightIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // 원래 그 자리에 있던 Active 타일의 UI부분(점수 표시)까지 삭제
                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisRightIndex].gameObject);
                        isMerged = true;
                        mergedTileAmount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[1] == Int32.MinValue)
                    continue;

                _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]] = _linkedActiveTile[yAxisIndex, xAxisIndex];
                _linkedActiveTile[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _linkedActiveTile[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileInfo>()._settedPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

                _linkAbleList.Add(_tileAmountInRow * yAxisIndex + xAxisIndex);
                _linkAbleList.Remove(_tileAmountInRow * arrivalPoint[0] + arrivalPoint[1]);
            }
        }
    }
}
