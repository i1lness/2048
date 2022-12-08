using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public TilesInfo _tilesInfo;
    public Transform[,] linkedActiveTile;
    public List<int> ableToLink;

    void Start()
    {
        linkedActiveTile = new Transform[_tilesInfo.tileAmountInRow, _tilesInfo.tileAmountInRow];
        ableToLink = new List<int>();

        for (int index = _tilesInfo.tileAmountInRow - 1; index >= 0; index--)
        {
            for (int jndex = _tilesInfo.tileAmountInRow - 1; jndex >= 0; jndex--)
            {
                ableToLink.Add(_tilesInfo.tileAmountInRow * (index) + jndex);
            }
        }

        MakeActiveTile();
        MakeActiveTile();
        MakeActiveTile();
        MakeActiveTile();
    }

    void Update()
    {
        
    }

    Transform MakeActiveTile()
    {
        Transform activeTile = Manager.Resource.Instantiate("ActiveTile").transform;

        int index = Random.Range(0, ableToLink.Count);
        int position = ableToLink[index];
        ableToLink.RemoveAt(index);

        int xPosition = position % _tilesInfo.tileAmountInRow;
        int yPosition = position / _tilesInfo.tileAmountInRow;

        linkedActiveTile[xPosition, yPosition] = activeTile;

        activeTile.position = _tilesInfo.tiles[xPosition, yPosition].position;
        activeTile.localScale = _tilesInfo.tiles[xPosition, yPosition].localScale;
        activeTile.GetComponent<SpriteRenderer>().color = Color.blue;

        return activeTile;
    }
}
