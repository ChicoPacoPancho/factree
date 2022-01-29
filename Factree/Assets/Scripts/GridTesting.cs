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

    [SerializeField] PlantsSO plantList;
    [SerializeField] Tile changeTile;
    [SerializeField] Tile startTile;
    [SerializeField] Tilemap cityMap;

    // Start is called before the first frame update
    void Start()
    {
        cityGrid = new CityGrid<CityMapGridObject>(20, 20, 1f, Vector3.zero, (CityGrid<CityMapGridObject> g, int x, int y) => new CityMapGridObject(g, x, y));
        
        cityVisual.SetGrid(cityGrid);

        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++)
            {
                //Vector3Int tile = cityMap.WorldToCell(new Vector3(i, j, 0));
                CityMapGridObject hmgo = cityGrid.GetGridObject(i, j);
                hmgo.BaseTile = startTile;
            }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tile = cityMap.WorldToCell(position);
            CityMapGridObject hmgo = cityGrid.GetGridObject(tile.x-5, tile.y-5);
            Debug.Log("Button Pressed: " + position);
            if (hmgo != null)
            {
                Debug.Log("Setting new tile:" + position);
                hmgo.ObjectTile = changeTile;
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
