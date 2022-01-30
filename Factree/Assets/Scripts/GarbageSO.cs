using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum GarbageTileType
{
    Dumpster,
    Observatory,
    Mall,
    Industry,
    Car,
    SkyScraper
}

[CreateAssetMenu(menuName = "Object/Garbage")]
public class GarbageSO : ScriptableObject
{


    public Tile baseImage;
    public ResourceItem resource;
    //public List<ResourceItem> resourceOut;
    //public List<ResourceItem> builtCost;
    //public List<BaseTileType> canBuildOn;
    //public List<SpawnType> spawnList;
    //public float spawnInterval;

    public void SubtractResource(int amount)
    {
        resource.count -= amount/60;

    }
}
