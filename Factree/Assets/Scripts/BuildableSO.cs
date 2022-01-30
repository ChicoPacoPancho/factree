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
    public int count;
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

    public bool CanBeBuiltOn(TileBase tile)
    {
        return canBuildOn.Contains(BuildableDictionary.Instance.GetTileType(tile));
    }
    public bool CheckCost()
    {
        foreach (ResourceItem ri in builtCost) {
            var available = ResourceManager.Instance.GetResourceAmountByType(ri.resourceType);
            if (available < ri.count)
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
            if (available < ri.count)
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
            ResourceManager.Instance.AddResourceAmountByType(ri.resourceType, -ri.count);
        }
    }
    public void AddIncome()
    {
        foreach (ResourceItem ri in resourceOut)
        {
            ResourceManager.Instance.AddResourceAmountByType(ri.resourceType, ri.count);
        }
    }
}
