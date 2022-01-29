using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CityMapGridVisual : MonoBehaviour
{
    CityGrid<CityMapGridObject> grid;
    [SerializeField] Tilemap cityMap;
    [SerializeField] Tilemap objectMap;


    // Temp variables
    public Tile baseTile;

    Mesh mesh;
    bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(CityGrid<CityMapGridObject> grid)
    {      
        this.grid = grid;
        Debug.Log("Grid set");
       
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
        Debug.Log("Updating visuals");
        for (x = 0; x < grid.Width; x++)
            for (y = 0; y < grid.Height; y++)
            {
                CityMapGridObject gridValue = grid.GetGridObject(x, y);
                cityMap.SetTile(new Vector3Int(x, y, 0), gridValue.BaseTile);
                objectMap.SetTile(new Vector3Int(x, y, 0), gridValue.ObjectTile);
                //int index = x * grid.Height + y;
                //Vector3 quadSize = new Vector3(1, 1) * grid.CellSize;

                //float stepX = x * grid.CellSize;
                //float stepY = y * grid.CellSize;

                //CityMapGridObject gridValue = grid.GetGridObject(x, y);
                //float gridValueNormalized = gridValue.GetValueNormalized();
                //Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                //Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX + grid.CellSize, stepY + grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                //Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX + grid.CellSize, stepY), new Color(gridValueNormalized, gridValueNormalized, .2f));
                //Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX, stepY + grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                //Debug.DrawLine(new Vector3(stepX + grid.CellSize, stepY), new Vector3(stepX + grid.CellSize, stepY + grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                //Debug.DrawLine(new Vector3(stepX + grid.CellSize, stepY), new Vector3(stepX, stepY + grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
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
        { baseTile = value; Debug.Log("Setting tile:" + x + "," + y); grid.TriggerGridObjectChanged(x, y);  }
    }
    //Plant/Building object
    Tile objectTile;
    public Tile ObjectTile
    {
        get
        { return objectTile; }
        set
        { objectTile = value; grid.TriggerGridObjectChanged(x, y); }
    }
    // Ph float
    // Soil Progress float
    // Resource object

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