using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInfo : MonoBehaviour
{
    public int tileAmountInRow = 4;

    void Start()
    {
        if (transform.Find("Board") == null)
            Manager.Resource.Instantiate("Board", transform);
    }

    public void MakeBoard(int tileAmountInRow)
    {
        this.tileAmountInRow = tileAmountInRow;

        if (transform.Find("Board") != null)
            Manager.Resource.Destroy(transform.Find("Board").gameObject);

        Manager.Resource.Instantiate("Board", transform);
    }
}
