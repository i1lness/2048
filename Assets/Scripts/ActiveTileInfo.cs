using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveTileInfo : MonoBehaviour
{
    public int _tileScore;

    float _lerpTime = 0.08f;
    float _currentLerpTime;

    private bool isArrived = false;

    private Vector3 _startPosition;
    public Vector3 _endPosition;

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
        _startPosition = transform.position;
        _endPosition = transform.position;
        _currentLerpTime = 0;
        isArrived = false;
    }

    void LateUpdate()
    {
        if (!isArrived)
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

        if ((_settedScale - transform.localScale).magnitude > 0.0001)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _settedScale, 0.1f);
        }
    }
}
