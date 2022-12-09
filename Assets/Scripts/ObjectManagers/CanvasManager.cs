using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    void Start()
    {
        InitialiseUICanvas(transform.GetComponent<Canvas>());
    }

    Canvas InitialiseUICanvas(Canvas canvas)
    {
        canvas.worldCamera = GameObject.Find("Sub Camera").GetComponent<Camera>();
        canvas.sortingLayerName = "ActionTileScoreUI";

        return canvas;
    }
}
