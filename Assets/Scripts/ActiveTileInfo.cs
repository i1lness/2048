using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveTileInfo : MonoBehaviour
{
    public int _tileScore;

    public Vector3 _settedPosition;
    private Vector3 _lastPosition;

    void Start()
    {
        InitialiseTileScore();
        ManualPositionUpdate();
    }

    void InitialiseTileScore()
    {
        _tileScore = 2;
        for (int index = Random.Range(0, 2); index > 0; index--)
        {
            _tileScore *= 2;
        }
    }

    public void ManualPositionUpdate()
    {
        _settedPosition = transform.position;
        _lastPosition = transform.position;
    }

    void Update()
    {
        if ((_settedPosition - _lastPosition).magnitude > 0.0001) // Ÿ���� ������ ���� ���� ��ġ�� �ٸ��� ����
        {
            transform.position = _settedPosition;
            _lastPosition = _settedPosition; // ���� �������� ������ ���� ���� ��ġ ����
        }
    }
}
