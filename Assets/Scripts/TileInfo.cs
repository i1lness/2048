using System.Collections.Generic;
using UnityEngine;

public class TilesInfo
{
    public Transform activeTilesCollection;
    public Transform[,] tiles;
    public Transform[,] linkedActiveTile;
    public List<int> ableToLink;
    public float boardSize;
    public int tileAmountInRow;
}
