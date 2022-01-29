using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct ResourceItem
{
    string name;
    int count;
}

public enum BaseTileType
{
    Concrete, 
    Water, 
    Grass, 
    Soil, 
    Asphalt
}

public enum SpawnType
{
    Bee,
    Goat
}

[CreateAssetMenu(menuName = "Buildable/Thing")]
public class BuildableSO : ScriptableObject
{
    public Tile baseImage;
    public List<ResourceItem> resourceIn;
    public List<ResourceItem> resourceOut;
    public List<ResourceItem> builtCost;
    public List<BaseTileType> canBuildOn;
    public List<SpawnType> spawnList;
    public float spawnInterval;
}
