using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveTilesManager : MonoBehaviour
{
    BoardManager _board;

    public static Action NoTileSpaceLeft;

    int _tileAmountInRow;

    Transform[,] _tiles;
    Transform[,] _activeTiles;

    List<int> _linkAbleList = new List<int>(); // 자리가 비워있는 타일의 _activeTiles의 index를 번호로 저장하는 곳

    void Start()
    {
        _board = transform.parent.GetComponent<BoardManager>();

        _tiles = _board.Tiles; // 객체 초기화 구문 (#1)
        _tileAmountInRow = _board.TileAmountInRow;

        _activeTiles = new Transform[_tileAmountInRow, _tileAmountInRow];

        for (int index = _tileAmountInRow * _tileAmountInRow - 1; index >= 0; index--)
        {
            _linkAbleList.Add(index);
        } // (#1) 여기까지

        MakeActiveTile();
        MakeActiveTile();

        InputManager.moveInputAction -= UpdateAllTilePosition;
        InputManager.moveInputAction += UpdateAllTilePosition;
    }

    private void OnDisable()
    {
        InputManager.moveInputAction -= UpdateAllTilePosition;
    }

    Transform MakeActiveTile()
    {
        if (_linkAbleList.Count <= 0 && NoTileSpaceLeft != null) // 만약 더 이상 타일놓을 자리가 없을 시 실행
        {
            NoTileSpaceLeft.Invoke();
            InputManager.moveInputAction -= UpdateAllTilePosition;
            return null;
        }


        Transform activeTile = Manager.Resource.Instantiate("ActiveTile").transform;
        activeTile.parent = transform;

        int randomNumber = UnityEngine.Random.Range(0, _linkAbleList.Count);
        int positionIndex = _linkAbleList[randomNumber]; // 쓸 index(비어있는 타일 번호) 불러오기
        _linkAbleList.RemoveAt(randomNumber);

        int yAxisIndex = positionIndex / _tileAmountInRow;
        int xAxisIndex = positionIndex % _tileAmountInRow;

        _activeTiles[yAxisIndex, xAxisIndex] = activeTile; // 배열로 activeTile를 저장

        activeTile.position = _tiles[yAxisIndex, xAxisIndex].position; // 만들어진 ActiveTile의 위치를 원래있던 타일틀과 맞추기
        activeTile.GetComponent<ActiveTileManager>().InitialisePositionVariable();

        activeTile.localScale = Vector3.zero; // 만들어진 ActiveTile의 소환 애니메이션 (팝업 효과)
        activeTile.GetComponent<ActiveTileManager>()._settedScale = _tiles[yAxisIndex, xAxisIndex].localScale;

        return activeTile;
    }

    void UpdateAllTilePosition(Define.MoveInputType moveInputType)
    {
        transform.GetComponent<AudioSource>().Play();

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
                if (_activeTiles[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { Int32.MinValue, xAxisIndex };

                bool isMerged = false;
                for (int yAxisUpperIndex = yAxisIndex - 1; yAxisUpperIndex - mergedTileAmount >= 0; yAxisUpperIndex--)
                {
                    if (_activeTiles[yAxisUpperIndex, xAxisIndex] == null)
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                    }
                    else if ((!isMerged) && (_activeTiles[yAxisUpperIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore == _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore))
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                        _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore *= 2;

                        Manager.Resource.Destroy(_activeTiles[yAxisUpperIndex, xAxisIndex].gameObject);
                        isMerged = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[0] == Int32.MinValue)
                    continue;

                if (isMerged)
                    mergedTileAmount = arrivalPoint[0] + 1;

                _activeTiles[arrivalPoint[0], arrivalPoint[1]] = _activeTiles[yAxisIndex, xAxisIndex];
                _activeTiles[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _activeTiles[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileManager>().InitialisePositionVariable();
                activeTile.GetComponent<ActiveTileManager>()._endPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

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
                if (_activeTiles[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { Int32.MinValue, xAxisIndex };

                bool isMerged = false;
                for (int yAxisDownIndex = yAxisIndex + 1; yAxisDownIndex + mergedTileAmount < _tileAmountInRow; yAxisDownIndex++)
                {
                    if (_activeTiles[yAxisDownIndex, xAxisIndex] == null)
                    {
                        arrivalPoint[0] = yAxisDownIndex;
                    }
                    else if ((!isMerged) && (_activeTiles[yAxisDownIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore == _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore))
                    {
                        arrivalPoint[0] = yAxisDownIndex;
                        _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore *= 2;

                        Manager.Resource.Destroy(_activeTiles[yAxisDownIndex, xAxisIndex].gameObject);
                        isMerged = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[0] == Int32.MinValue)
                    continue;

                if (isMerged)
                    mergedTileAmount = _tileAmountInRow - arrivalPoint[0];

                _activeTiles[arrivalPoint[0], arrivalPoint[1]] = _activeTiles[yAxisIndex, xAxisIndex];
                _activeTiles[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _activeTiles[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileManager>().InitialisePositionVariable();
                activeTile.GetComponent<ActiveTileManager>()._endPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

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
                if (_activeTiles[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { yAxisIndex, Int32.MinValue };

                bool isMerged = false;
                for (int xAxisLeftIndex = xAxisIndex - 1; xAxisLeftIndex - mergedTileAmount >= 0; xAxisLeftIndex--)
                {
                    if (_activeTiles[yAxisIndex, xAxisLeftIndex] == null)
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                    }
                    else if ((!isMerged) && (_activeTiles[yAxisIndex, xAxisLeftIndex].GetComponent<ActiveTileManager>()._tileScore == _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                        _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore *= 2;

                        Manager.Resource.Destroy(_activeTiles[yAxisIndex, xAxisLeftIndex].gameObject);
                        isMerged = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[1] == Int32.MinValue)
                    continue;

                if (isMerged)
                    mergedTileAmount = arrivalPoint[1] + 1;

                _activeTiles[arrivalPoint[0], arrivalPoint[1]] = _activeTiles[yAxisIndex, xAxisIndex];
                _activeTiles[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _activeTiles[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileManager>().InitialisePositionVariable();
                activeTile.GetComponent<ActiveTileManager>()._endPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

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
                if (_activeTiles[yAxisIndex, xAxisIndex] == null)
                    continue;

                int[] arrivalPoint = new int[2] { yAxisIndex, Int32.MinValue };

                bool isMerged = false;
                for (int xAxisRightIndex = xAxisIndex + 1; xAxisRightIndex + mergedTileAmount < _tileAmountInRow; xAxisRightIndex++)
                {
                    if (_activeTiles[yAxisIndex, xAxisRightIndex] == null)
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                    }
                    else if ((!isMerged) && (_activeTiles[yAxisIndex, xAxisRightIndex].GetComponent<ActiveTileManager>()._tileScore == _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                        _activeTiles[yAxisIndex, xAxisIndex].GetComponent<ActiveTileManager>()._tileScore *= 2;

                        Manager.Resource.Destroy(_activeTiles[yAxisIndex, xAxisRightIndex].gameObject);
                        isMerged = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (arrivalPoint[1] == Int32.MinValue)
                    continue;

                if (isMerged)
                    mergedTileAmount = _tileAmountInRow - arrivalPoint[1];

                _activeTiles[arrivalPoint[0], arrivalPoint[1]] = _activeTiles[yAxisIndex, xAxisIndex];
                _activeTiles[yAxisIndex, xAxisIndex] = null;

                Transform activeTile = _activeTiles[arrivalPoint[0], arrivalPoint[1]];
                activeTile.GetComponent<ActiveTileManager>().InitialisePositionVariable();
                activeTile.GetComponent<ActiveTileManager>()._endPosition = _tiles[arrivalPoint[0], arrivalPoint[1]].position;

                _linkAbleList.Add(_tileAmountInRow * yAxisIndex + xAxisIndex);
                _linkAbleList.Remove(_tileAmountInRow * arrivalPoint[0] + arrivalPoint[1]);
            }
        }
    }
}
