using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Transform _board;

    void Start()
    {
        if (transform.Find("Board") == null) // _board Á¤ÀÇ
        {
            _board = Manager.Resource.Instantiate("Board", transform).transform;
        }
        else
        {
            _board = transform.Find("Board");
        }
    }
}
