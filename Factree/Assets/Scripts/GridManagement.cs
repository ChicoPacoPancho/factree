using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class GridManagement : MonoBehaviour
{
    public event EventHandler<OnSelectionButtonEventArgs> OnSelectionButtonClicked;
    public class OnSelectionButtonEventArgs : EventArgs
    {
        public Sprite selectedTile;
    }
        
    CityGrid<bool>[] grid;
    public CityGrid<CityMapGridObject> cityGrid;
    [SerializeField] CityMapGridVisual cityVisual;
    [SerializeField] float scale;

    [SerializeField] PlantsSO plantList;
    [SerializeField] Tile changeTile;
    [SerializeField] Tile startTile;
    [SerializeField] Tilemap cityMap;
    [SerializeField] GameObject selectionSquare;

    // Start is called before the first frame update
    void Start()
    {
        this.OnSelectionButtonClicked += OnTileSelected;

        cityGrid = new CityGrid<CityMapGridObject>(12, 12, 1f, Vector3.zero, (CityGrid<CityMapGridObject> g, int x, int y) => new CityMapGridObject(g, x, y));
        
        cityVisual.SetGrid(cityGrid);

        for (int i = 0; i < 12; i++)
            for (int j = 0; j < 12; j++)
            {
                int newI, newJ;
                newI = 5 - i;
                newJ = 5 - j;
                if (newI * newI + newJ * newJ < 30)
                {
                    //Vector3Int tile = cityMap.WorldToCell(new Vector3(i, j, 0));
                    CityMapGridObject hmgo = cityGrid.GetGridObject(i, j);
                    hmgo.BaseTile = startTile;
                }
            }
    }

    void OnTileSelected(object sender, OnSelectionButtonEventArgs e )
    {
        changeTile = startTile;
    }

    private void Update()
    {
        // Find mouse position on Grid
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        Vector3Int tile = cityMap.WorldToCell(position);
        CityMapGridObject hmgo = cityGrid.GetGridObject(tile.x, tile.y);

        selectionSquare.SetActive(false);

        if (hmgo == null)
        {
            
            return;
        }

        // Do not update selection if over the UI Panels
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (hmgo.BaseTile != null)
        {
            selectionSquare.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Button Pressed: " + position);
                if (hmgo != null)
                {
                    Debug.Log("Setting new tile:" + position);
                    hmgo.ObjectTile = changeTile;
                }
            }

            // Get the position of the mouse and convert it to cells
            if (hmgo.BaseTile != null)
            {                
                Vector3 roundedPos = cityMap.CellToWorld(tile);
                selectionSquare.transform.position = roundedPos;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     
    }

    public void TriggerSelectionButtonClicked(Sprite selection)
    {
        if (OnSelectionButtonClicked != null)
        {
            OnSelectionButtonClicked(this, new OnSelectionButtonEventArgs { selectedTile = selection });
        }
    }
}
