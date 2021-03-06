using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct ResourceItem
{
    [SerializeField]
    public ResourceType resourceType;
    [SerializeField]
    public float count;
}

public enum BaseTileType
{
    Concrete,
    Water,
    Grass,
    Soil,
    Asphalt,
    None
}



public enum BuildableTileType
{
    TheGreatTree,
    BeeTree,
    BerryBush,
    Caragana,
    GiantFlyTrap,
    MangroveTree,
    RootTree,
    Solar,
    StorageGrove,
    WaterWheel,
    WreackingBallTree
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
    Goat,
    Squirrel
}

[CreateAssetMenu(menuName = "Object/Buildable")]
public class BuildableSO : ScriptableObject
{
    public Tile baseImage;
    public List<ResourceItem> resourceIn;
    public List<ResourceItem> resourceOut;
    public List<ResourceItem> builtCost;
    public List<BaseTileType> canBuildOn;
    public List<GameObject> spawnList;
    public float spawnInterval;
    public List<CollectResourceAbility> collectResourceAbilities;

    public bool removesPlants = false;

    [TextArea(1, 1)]
    public string displayName = "";

    [TextArea(3, 5)]
    public string shortDescription = "";

    public bool CanBeBuiltOn(Tile tile)
    {
        return canBuildOn.Contains(GroundDictionary.Instance.GetTileType(tile));
    }
    public bool CheckCost()
    {
        foreach (ResourceItem ri in builtCost) {
            var available = ResourceManager.Instance.GetResourceAmountByType(ri.resourceType);
            if (available <= ri.count)
            {
                return false;
            }
        }
        return true;
    }
    public void SubtractCost()
    {
        foreach (ResourceItem ri in builtCost)
        {
            ResourceManager.Instance.AddResourceAmountByType(ri.resourceType, -ri.count);
        }
    }
    public bool CheckUpkeep()
    {
        foreach (ResourceItem ri in resourceIn)
        {
            var available = ResourceManager.Instance.GetResourceAmountByType(ri.resourceType);
            if (available <= ri.count/60f)
            {
                return false;
            }
        }
        return true;
    }
    public void SubtractUpkeep()
    {
        foreach (ResourceItem ri in resourceIn)
        {
            ResourceManager.Instance.AddResourceAmountByType(ri.resourceType, -ri.count/60f);
        }
    }
    public void AddIncome()
    {
        foreach (ResourceItem ri in resourceOut)
        {
            //Debug.Log("Adding " + ri.count / 60f + ri.resourceType + " for a total of " + ResourceManager.Instance.GetResourceAmountByType(ri.resourceType));
            ResourceManager.Instance.AddResourceAmountByType(ri.resourceType, ri.count/60f);
        }
    }

    public void SpawnSpawns(Vector2 position)
    {
        foreach (GameObject obj in spawnList)
        {
            Instantiate(obj, position, Quaternion.identity, null);
        }
    }

    public void DoAbilitiesOnTarget(int x, int y, int targetX, int targetY)
    {
        foreach(CollectResourceAbility ca in collectResourceAbilities)
        {
            ca.DoAbility(x, y, targetX, targetY);
        }
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
