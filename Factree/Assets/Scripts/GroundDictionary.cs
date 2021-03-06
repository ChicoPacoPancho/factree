using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDictionary : MonoBehaviour
{
    [System.Serializable]
    public struct BaseTileDictionary
    {
        [SerializeField]
        public BaseTileType tileType;
        [SerializeField]
        public Tile tile;
    }

    public List<BaseTileDictionary> dictionary;

    public static GroundDictionary Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public Tile GetTile(BaseTileType type)
    {
        foreach (var dict in dictionary)
        {
            if (dict.tileType == type)
            {
                return dict.tile;
            }
        }
        return null;
    }

    public BaseTileType GetTileType(Tile tile)
    {
        foreach (var dict in dictionary)
        {
            if (dict.tile == tile)
            {
                return dict.tileType;
            }
        }
        return BaseTileType.Concrete;
    }
}
