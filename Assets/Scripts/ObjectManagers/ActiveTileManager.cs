using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActiveTileManager : MonoBehaviour
{
    public int _tileScore;
    private int _lastTileScore;

    float _lerpTime = 0.08f;
    float _currentLerpTime;

    private bool isArrived = false;

    private Vector3 _startPosition;
    public Vector3 _endPosition;

    public Vector3 _settedScale;

    public Action<int> TileUIInfoUpdate = null;

    void Start()
    {
        InitialiseTileScore();
        ChangeTileColorByTileScore();
    }

    /* 타일 점수 초기설정 함수 */
    void InitialiseTileScore()
    {
        _tileScore = 2;
        for (int index = Random.Range(0, 2); index > 0; index--)
        {
            _tileScore *= 2;
        }
    }

    /* 타일 위치관련 변수 초기화 함수 */
    public void InitialisePositionVariable()
    {
        _startPosition = transform.position;
        _endPosition = transform.position;
        _currentLerpTime = 0;
        isArrived = false;
    }

    void LateUpdate()
    {
        if (_lastTileScore != _tileScore)
        {
            ChangeTileColorByTileScore();

            _lastTileScore = _tileScore;
        }

        if (!isArrived)
        {
            MoveAnimation();
        }

        if ((_settedScale - transform.localScale).magnitude > 0.0001)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _settedScale, 0.1f);
        }
    }

    public void ChangeTileColorByTileScore()
    {
        if (_tileScore <= 32)
        {
            int tempTileScore = _tileScore;
            float colorDiff = 0;
            while (tempTileScore > 1)
            {
                colorDiff++;
                tempTileScore /= 2;
            }
            colorDiff *= 0.1f;
            transform.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f - colorDiff, 1);
        }
        else //if (_tileScore <= 512)
        {
            int tempTileScore = _tileScore;
            float colorDiff = 0;
            while (tempTileScore > 1)
            {
                colorDiff++;
                tempTileScore /= 2;
            }
            colorDiff -= 5;
            colorDiff *= 0.08f;
            transform.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f - colorDiff * 1.5f, 0.4f - colorDiff, 1);
        }
        //else
        //{
        //    int tempTileScore = _tileScore;
        //    float colorDiff = 0;
        //    while (tempTileScore > 1)
        //    {
        //        colorDiff++;
        //        tempTileScore /= 2;
        //    }
        //    colorDiff -= 9;
        //    colorDiff *= 0.03f;
        //    if (colorDiff > 0.18f)
        //        colorDiff = 0.18f;
        //    transform.GetComponent<SpriteRenderer>().color = new Color(0.9f - colorDiff, 0.18f - colorDiff, 0.3f - colorDiff, 1);
        //}

        if (TileUIInfoUpdate != null)
            TileUIInfoUpdate.Invoke(_tileScore);
    }

    void MoveAnimation() // 움직이는 애니메이션 구현
    {
        _currentLerpTime += Time.deltaTime;

        if (_currentLerpTime > _lerpTime)
        {
            _currentLerpTime = _lerpTime;
            isArrived = true;
        }

        float t = _currentLerpTime / _lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);

        transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
    }
}
