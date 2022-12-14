using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTileUICanvasManager : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = transform.GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("Sub Camera").GetComponent<Camera>();
        canvas.sortingLayerName = "ActionTileScoreUI";
    }
}
