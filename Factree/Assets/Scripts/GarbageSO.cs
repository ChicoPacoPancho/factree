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
    SkyScraper,
    None
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

    [TextArea(1, 1)]
    public string displayName = "";

    [TextArea(3, 5)]
    public string shortDescription = "";

    public void SubtractResource(float amount)
    {
        resource.count -= amount/60;

    }

    public string GetDisplayName()
    {
        if (displayName == null || displayName == "")
        {
            // Convert "CamelCase" to "Display Case"
            return System.Text.RegularExpressions.Regex.Replace(name, "([A-Z])", " $1",
                System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        return displayName;
    }

    public string GetDescription()
    {
        return shortDescription;
    }
}
