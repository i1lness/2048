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
    private List<int> _linkAbleList = new List<int>(); // �ڸ��� ����ִ� Ÿ���� _linkedActiveTile�� index�� ��ȣ�� �����ϴ� ��

    void Start()
    {
        _tiles = _board.Tiles; // ��ü �ʱ�ȭ ���� (#1)
        _tileAmountInRow = _board.TileAmountInRow;

        _linkedActiveTile = new Transform[_tileAmountInRow, _tileAmountInRow];

        for (int index = _tileAmountInRow * _tileAmountInRow - 1; index >= 0; index--)
        {
            _linkAbleList.Add(index);
        } // (#1) �������

        _scoreUICanvas = Manager.Resource.Instantiate("ScoreUICanvas", _board.transform).transform; // Ÿ�� ���� ǥ���� UI�� ��Ƶ� �� ���� �� ����

        MakeActiveTile();
        MakeActiveTile();

        Manager.Input.moveInputAction += UpdateAllTilePosition;
    }

    Transform MakeActiveTile()
    {
        Transform activeTile = Manager.Resource.Instantiate("ActiveTile").transform;
        activeTile.parent = transform;

        int randomNumber = UnityEngine.Random.Range(0, _linkAbleList.Count);
        int positionIndex = _linkAbleList[randomNumber]; // �� index(����ִ� Ÿ�� ��ȣ) �ҷ�����
        _linkAbleList.RemoveAt(randomNumber);

        int yAxisIndex = positionIndex / _tileAmountInRow;
        int xAxisIndex = positionIndex % _tileAmountInRow;

        _linkedActiveTile[yAxisIndex, xAxisIndex] = activeTile; // �迭�� activeTile�� ����

        activeTile.position = _tiles[yAxisIndex, xAxisIndex].position; // ������� ActiveTile�� ��ġ�� ũ�⸦ ���� �ִ� Ÿ��Ʋ�� ���߱�
        activeTile.localScale = _tiles[yAxisIndex, xAxisIndex].localScale;

        activeTile.GetComponent<ActiveTileInfo>().ManualPositionUpdate(); // ������ Ÿ���� ��ġ�� ������ �ٽ� ����

        activeTile.GetComponent<SpriteRenderer>().color = Color.blue; // TEMP
        
        activeTile.GetComponent<ActiveTileScoreUIConnecter>().InitialiseController(_scoreUICanvas); // ��Ʈ�ѷ� �ʱ�ȭ

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

                int[] arrivalPoint = new int[2] {Int32.MinValue, xAxisIndex };

                bool isMerged = false;
                for (int yAxisUpperIndex = yAxisIndex - 1 - mergedTileAmount; yAxisUpperIndex >= 0; yAxisUpperIndex--)
                {
                    if (_linkedActiveTile[yAxisUpperIndex, xAxisIndex] == null)
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisUpperIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[0] = yAxisUpperIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisUpperIndex, xAxisIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // ���� �� �ڸ��� �ִ� Active Ÿ���� UI�κ�(���� ǥ��)���� ����
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
                for (int yAxisDownIndex = yAxisIndex + 1 + mergedTileAmount; yAxisDownIndex < _tileAmountInRow; yAxisDownIndex++)
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
                for (int xAxisLeftIndex = xAxisIndex - 1 - mergedTileAmount; xAxisLeftIndex >= 0; xAxisLeftIndex--)
                {
                    if (_linkedActiveTile[yAxisIndex, xAxisLeftIndex] == null)
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisIndex, xAxisLeftIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisLeftIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisLeftIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // ���� �� �ڸ��� �ִ� Active Ÿ���� UI�κ�(���� ǥ��)���� ����
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
                for (int xAxisRightIndex = xAxisIndex + 1 + mergedTileAmount; xAxisRightIndex < _tileAmountInRow; xAxisRightIndex++)
                {
                    if (_linkedActiveTile[yAxisIndex, xAxisRightIndex] == null)
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                    }
                    else if ((!isMerged) && (_linkedActiveTile[yAxisIndex, xAxisRightIndex].GetComponent<ActiveTileInfo>()._tileScore == _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore))
                    {
                        arrivalPoint[1] = xAxisRightIndex;
                        _linkedActiveTile[yAxisIndex, xAxisIndex].GetComponent<ActiveTileInfo>()._tileScore *= 2;

                        Manager.Resource.Destroy(_linkedActiveTile[yAxisIndex, xAxisRightIndex].GetComponent<ActiveTileScoreUIConnecter>().ConnectedUI.gameObject); // ���� �� �ڸ��� �ִ� Active Ÿ���� UI�κ�(���� ǥ��)���� ����
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
