using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CityMapGridVisual : MonoBehaviour
{
    public CityGrid<CityMapGridObject> grid;
    [SerializeField] Tilemap cityMap;
    [SerializeField] Tilemap objectMap;
    
    // Temp variables
    public Tile baseTile;
    

    private void Awake()
    {
      
    }

    public void SetGrid(CityGrid<CityMapGridObject> grid)
    {      
        this.grid = grid;
               
        UpdateCityMapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, CityGrid<CityMapGridObject>.OnGridObjectChangedEventArgs e)
    {
        UpdateCityMapVisual();
    }

  
    private void UpdateCityMapVisual()
    {
        int x, y;

        for (x = 0; x < grid.Width; x++)
            for (y = 0; y < grid.Height; y++)
            {
                CityMapGridObject gridValue = grid.GetGridObject(x, y);
                cityMap.SetTile(new Vector3Int(x, y, 0), gridValue.BaseTile);

                if (gridValue.PlantTile == null && gridValue.Resource == null)
                {
                    objectMap.SetTile(new Vector3Int(x, y, 0), null);
                    continue;
                }

                if (gridValue.PlantTile != null)
                {
                    objectMap.SetTile(new Vector3Int(x, y, 0), gridValue.PlantTile.baseImage);
                }
                if (gridValue.Resource != null)
                {
                    objectMap.SetTile(new Vector3Int(x, y, 0), gridValue.Resource.baseImage);
                }
                //else
                //{
                //    if (gridValue.Resource == null)
                //    {
                //        objectMap.SetTile(new Vector3Int(x, y, 0), null);
                //    }
                //}

            }

        
    }
}

public class CityMapGridObject
{

    //Base Tile object
    Tile baseTile;
    public Tile BaseTile
    {
        get
        { return baseTile; }
        set
        { baseTile = value; grid.TriggerGridObjectChanged(x, y); }
    }
    //Plant object
    BuildableSO plantTile;
    public BuildableSO PlantTile
    {
        get
        { return plantTile; }
        set
        { plantTile = value; grid.TriggerGridObjectChanged(x, y); }
    }
    // Ph float
    // Soil Progress float
    // Resource object
    GarbageSO resource;
    public GarbageSO Resource
    {
        get { return resource; }
        set { resource = value; grid.TriggerGridObjectChanged(x, y); }
    }

    const int MIN = 0;
    const int MAX = 100;

    private CityGrid<CityMapGridObject> grid;
    public int x, y, value;

    public CityMapGridObject(CityGrid<CityMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }



    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}