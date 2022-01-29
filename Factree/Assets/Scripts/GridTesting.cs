using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    Grid<bool>[] grid;
    public Grid<HeatMapGridObject> heatGrid;
    [SerializeField] HeatmapGridVisual heatVisual;
    [SerializeField] float scale;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<bool>[3];
        grid[0] = new Grid<bool>(20, 10, scale, transform.position, (Grid<bool> g, int x, int y) => false);
        grid[1] = new Grid<bool>(10, 10, scale * 1.5f, Vector3.zero, (Grid<bool> g, int x, int y) => false);
        grid[2] = new Grid<bool>(2, 10, scale * .5f, Vector3.forward, (Grid<bool> g, int x, int y) => false);

        heatGrid = new Grid<HeatMapGridObject>(20, 10, 1f, Vector3.zero, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        heatVisual.SetGrid(heatGrid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HeatMapGridObject hmgo = heatGrid.GetGridObject(position);
            if (hmgo != null)
            {
                hmgo.AddValue(5);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        heatGrid.DrawDebug(1f);
        //for (int i = 0; i < 3; i++)
        //{
        //    grid[i].DrawDebug(scale*(i+.5f));
        //}
    }
}
