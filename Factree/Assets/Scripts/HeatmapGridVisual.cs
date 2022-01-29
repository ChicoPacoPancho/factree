using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGridVisual : MonoBehaviour
{
    Grid<HeatMapGridObject> grid;
    Mesh mesh;
    bool updateMesh;
    
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid( Grid<HeatMapGridObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridObjectChangedEventArgs e)
    {
        updateMesh = true;
    }
        
    void LateUpdate()
    {
       
            UpdateHeatMapVisual();
      
    }

    private void UpdateHeatMapVisual()
    {
        int x, y;

        for(x=0;x<grid.Width; x++)
            for(y=0;y<grid.Height;y++)
            {
                int index = x * grid.Height + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.CellSize;

                float stepX = x * grid.CellSize;
                float stepY = y * grid.CellSize;

                HeatMapGridObject gridValue = grid.GetGridObject(x, y);
                float gridValueNormalized = gridValue.GetValueNormalized();
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX + grid.CellSize, stepY + grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX + grid.CellSize, stepY), new Color(gridValueNormalized, gridValueNormalized, .2f));
                Debug.DrawLine(new Vector3(stepX, stepY), new Vector3(stepX, stepY+ grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                Debug.DrawLine(new Vector3(stepX + grid.CellSize, stepY), new Vector3(stepX + grid.CellSize, stepY+ grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
                Debug.DrawLine(new Vector3(stepX + grid.CellSize, stepY), new Vector3(stepX, stepY+ grid.CellSize), new Color(gridValueNormalized, gridValueNormalized, .2f));
            }
    }
}

public class HeatMapGridObject
{
    const int MIN = 0;
    const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    public int x, y, value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y )
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int addValue)
    {
        value += addValue;
        value += Mathf.Clamp(addValue, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
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