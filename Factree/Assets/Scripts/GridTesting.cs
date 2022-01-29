using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTesting : MonoBehaviour
{
    CityGrid<bool>[] grid;
    public CityGrid<CityMapGridObject> cityGrid;
    [SerializeField] CityMapGridVisual cityVisual;
    [SerializeField] float scale;

    [SerializeField] Tile changeTile;
    [SerializeField] Tile startTile;

    // Start is called before the first frame update
    void Start()
    {
        cityGrid = new CityGrid<CityMapGridObject>(20, 10, 1f, Vector3.zero, (CityGrid<CityMapGridObject> g, int x, int y) => new CityMapGridObject(g, x, y));

        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                CityMapGridObject hmgo = cityGrid.GetGridObject(new Vector3(i,j));
                hmgo.BaseTile = changeTile;
            }

        cityVisual.SetGrid(cityGrid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CityMapGridObject hmgo = cityGrid.GetGridObject(position);
            if (hmgo != null)
            {
                hmgo.BaseTile = changeTile;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     //   cityGrid.DrawDebug(1f);
        //for (int i = 0; i < 3; i++)
        //{
        //    grid[i].DrawDebug(scale*(i+.5f));
        //}
    }
}
