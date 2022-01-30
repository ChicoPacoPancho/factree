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
        public GarbageSO item;
    }

    public List<GarbageTileDictionary> dictionary;

    public static GarbageDictionary Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public GarbageSO GetTile(GarbageTileType type)
    {
        foreach (var dict in dictionary)
        {
            if (dict.tileType == type)
            {
                return dict.item;
            }
        }
        return null;
    }

    public GarbageTileType GetTileType(GarbageSO item)
    {
        foreach (var dict in dictionary)
        {
            if (dict.item == item)
            {
                return dict.tileType;
            }
        }
        return GarbageTileType.Car;
    }
}
