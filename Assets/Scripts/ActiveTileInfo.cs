using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveTileInfo : MonoBehaviour
{
    public int _tileScore;

    public Vector3 _settedPosition;

    public Vector3 _settedScale;

    void Start()
    {
        InitialiseTileScore();
    }

    void InitialiseTileScore()
    {
        _tileScore = 2;
        for (int index = Random.Range(0, 2); index > 0; index--)
        {
            _tileScore *= 2;
        }
    }

    public void InitialisePositionVariable()
    {
        _settedPosition = transform.position;
    }

    void Update()
    {
        if ((_settedScale - transform.localScale).magnitude > 0.0001)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _settedScale, 0.1f);
        }
    }

    void LateUpdate()
    {
        if ((_settedPosition - transform.position).magnitude > 0.0001) // 타일이 가야할 곳이 현재 위치와 다르면 실행
        {
            transform.position = Vector3.Lerp(transform.position, _settedPosition, 0.1f);
        }
    }
}
