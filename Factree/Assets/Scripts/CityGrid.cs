using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGrid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs: EventArgs
    {
        public int x;
        public int y;
    }


    private int width;
    private int height;
    private Vector3 origin;
    private float cellSize;
    private TGridObject[,] gridArray;

    public int Width { get { return width; }  set { width = value; } }
    public int Height { get { return height; } set { height = value; } }
    public float CellSize { get { return cellSize; } set { cellSize = value;     } }

    public CityGrid(int width, int height, float cellSize, Vector3 origin, Func<CityGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        int x, y;

        this.width = width;
        this.height = height;
        this.origin = origin;
        this.cellSize = cellSize;

        gridArray = new TGridObject[width, height];

        for (x = 0; x < width; x++)
        {
            for (y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    //private Vector3 GetWorldPosition(int x, int y)
    //{
    //    return new Vector3(x, y) * cellSize + origin;
    //}

    //private void GetXY(Vector3 worldPosition, out int x, out int y)
    //{
    //    x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
    //    y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    //}

    //public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
    //{
    //    int x, y;

    //    GetXY(worldPosition, out x, out y);
    //    SetGridObject(x, y, gridObject);
    //}

    public void SetGridObject(int x, int y, TGridObject gridObject)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = gridObject;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged( int x, int y)
    {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    //public TGridObject GetGridObject(Vector3 worldPosition)
    //{
    //    int x, y;
    //    GetXY(worldPosition, out x, out y);
    //    return GetGridObject(x, y);
    //}

    public void DrawDebug(float scale)
    {
        int x, y;
        cellSize = scale;
        float halfCell = scale / 2.0f;
        Vector3 pos = origin + new Vector3(0.5f,0);

        for (x = 0; x < width; x++)
            for (y = 0; y < height; y++)
            {
                //Debug.DrawLine(pos + new Vector3(x, y + 1) * cellSize, pos + new Vector3(x + 1, y + 1) * cellSize, Color.red);
                //Debug.DrawLine(pos + new Vector3(x + 1, y + 1) * cellSize, pos + new Vector3(x + 1, y) * cellSize, Color.blue);


                Debug.DrawLine(pos + new Vector3(x+(y*cellSize), (x * cellSize) - y -halfCell) * cellSize, pos + new Vector3(x + (y * cellSize)+cellSize, (x * cellSize) - y +halfCell ) * cellSize, Color.red);
                Debug.DrawLine(pos + new Vector3(x + (y * cellSize), (x*cellSize)- y - halfCell) * cellSize, pos + new Vector3(x + (y * cellSize) + cellSize, (x * cellSize) - y - cellSize-halfCell) * cellSize, Color.blue);
                //Debug.DrawLine(pos + new Vector3(x + 1, y + 1), pos + new Vector3(x + halfCell + 1, y), Color.blue);

                Debug.Log("pos: " + (pos + new Vector3(x + (y * cellSize) , y - halfCell) * cellSize));
            }
        //Debug.DrawLine(new Vector3(pos.x+halfCell, pos.y, pos.z), pos + new Vector3(width + halfCell, 0) * cellSize, Color.red);
        //Debug.DrawLine(new Vector3(pos.x+halfCell, pos.y, pos.z), pos + new Vector3(0 + halfCell, height) * cellSize, Color.blue);

    }

}
