using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct ResourceItem
{
    [SerializeField]
    ResourceType resourceType;
    [SerializeField]
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

public enum ResourceType
{
    Water,
    Energy,
    Nitrogen,
    Sulfur,
    Potassium,
    Phosphorous
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
