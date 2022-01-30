using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildableDictionary : MonoBehaviour
{
    [System.Serializable]
    public struct BaseTileDictionary
    {        
        public BuildableTileType tileType;        
        public BuildableSO tile;
    }

    public List<BaseTileDictionary> dictionary;

    public static BuildableDictionary Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    
    public BuildableSO GetTile(BuildableTileType type)
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

    public BuildableTileType GetTileType(Tile tile)
    {
        foreach (var dict in dictionary)
        {
            if (dict.tile == tile)
            {
                return dict.tileType;
            }
        }
        return BuildableTileType.BeeTree;
    }

}
