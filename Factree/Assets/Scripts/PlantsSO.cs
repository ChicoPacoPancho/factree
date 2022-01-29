using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Plantlist/Plant")]
public class PlantsSO : ScriptableObject
{
    public List<Tile> plantList;
}
