using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GarbageDictionary : MonoBehaviour
{
    [System.Serializable]
    public struct GarbageTileDictionary
    {
        public GarbageTileType tileType;
        public Tile tile;
    }

    public List<GarbageTileDictionary> dictionary;

    public static GarbageDictionary Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public Tile GetTile(GarbageTileType type)
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

    public GarbageTileType GetTileType(Tile tile)
    {
        foreach (var dict in dictionary)
        {
            if (dict.tile == tile)
            {
                return dict.tileType;
            }
        }
        return GarbageTileType.Car;
    }
}
