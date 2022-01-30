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
        public BuildableSO selectedSO;
    }
        
    
    public CityGrid<CityMapGridObject> cityGrid;
    [SerializeField] CityMapGridVisual cityVisual;
    [SerializeField] float scale;
    
    [SerializeField] PlantsSO plantList;
    [SerializeField] BuildableSO changeSO;
    [SerializeField] Tile startTile;
    [SerializeField] Tilemap cityMap;
    [SerializeField] GameObject selectionSquare;
    [SerializeField] List<Tile> baseTiles;

    // Start is called before the first frame update
    void Start()
    {
        int j, i;
        this.OnSelectionButtonClicked += OnTileSelected;

        cityGrid = new CityGrid<CityMapGridObject>(12, 12, 1f, Vector3.zero, (CityGrid<CityMapGridObject> g, int x, int y) => new CityMapGridObject(g, x, y));
        
        cityVisual.SetGrid(cityGrid);

        for (i = 0; i < 12; i++)
            for (j = 0; j < 12; j++)
            {
                int newI, newJ;
                newI = 5 - i;
                newJ = 5 - j;


                if (newI * newI + newJ * newJ < 30)
                {
                    //Vector3Int tile = cityMap.WorldToCell(new Vector3(i, j, 0));
                    CityMapGridObject hmgo = cityGrid.GetGridObject(i, j);
                    hmgo.BaseTile = baseTiles[3];
                }
            }

        // Build up base tiles
        // Road
        for (i=3; i<=4; i++)
            for(j=0;j<=10;j++)            
                cityGrid.GetGridObject(i, j).BaseTile = baseTiles[2];
        // Soil
        for (i = 7; i <= 9; i++)
            for (j = 3; j <= 5; j++)
                cityGrid.GetGridObject(i, j).BaseTile = baseTiles[1];
        // World Tree
        cityGrid.GetGridObject(8, 4).BaseTile = baseTiles[0];

        // Water
        for (j = 3; j <= 7; j++)
            cityGrid.GetGridObject(10, j).BaseTile = baseTiles[4];
        cityGrid.GetGridObject(9, 2).BaseTile = baseTiles[4];
        cityGrid.GetGridObject(8, 1).BaseTile = baseTiles[4];
        cityGrid.GetGridObject(7, 0).BaseTile = baseTiles[4];



        // Build up Objects
        // World Tree
        cityGrid.GetGridObject(8, 4).ObjectTile = plantList.plantList[6];
        // Dumpster
        cityGrid.GetGridObject(8, 6).ObjectTile = plantList.plantList[0];
        cityGrid.GetGridObject(8, 5).ObjectTile = plantList.plantList[0];
        cityGrid.GetGridObject(2, 6).ObjectTile = plantList.plantList[0];
        cityGrid.GetGridObject(2, 5).ObjectTile = plantList.plantList[0];

        // Observatory
        cityGrid.GetGridObject(5, 2).ObjectTile = plantList.plantList[1];
        cityGrid.GetGridObject(7, 5).ObjectTile = plantList.plantList[1];
        cityGrid.GetGridObject(1, 7).ObjectTile = plantList.plantList[1];
        cityGrid.GetGridObject(1, 6).ObjectTile = plantList.plantList[1];
        cityGrid.GetGridObject(0, 5).ObjectTile = plantList.plantList[1];

        // Mall
        cityGrid.GetGridObject(1, 3).ObjectTile = plantList.plantList[2];
        cityGrid.GetGridObject(1, 4).ObjectTile = plantList.plantList[2];
        cityGrid.GetGridObject(9, 5).ObjectTile = plantList.plantList[2];
        cityGrid.GetGridObject(7, 3).ObjectTile = plantList.plantList[2];

        // Observatory
        cityGrid.GetGridObject(8, 9).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(5, 9).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(8, 7).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(6, 1).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(6, 0).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(5, 0).ObjectTile = plantList.plantList[3];
        cityGrid.GetGridObject(5, 1).ObjectTile = plantList.plantList[3];

        // Car
        cityGrid.GetGridObject(7, 9).ObjectTile = plantList.plantList[4];
        cityGrid.GetGridObject(7, 8).ObjectTile = plantList.plantList[4];
        cityGrid.GetGridObject(6, 8).ObjectTile = plantList.plantList[4];
        cityGrid.GetGridObject(5, 8).ObjectTile = plantList.plantList[4];
        cityGrid.GetGridObject(6, 6).ObjectTile = plantList.plantList[4];        
        cityGrid.GetGridObject(6, 5).ObjectTile = plantList.plantList[4];
        cityGrid.GetGridObject(4, 6).ObjectTile = plantList.plantList[4];

        // Skyscraper
        cityGrid.GetGridObject(2, 7).ObjectTile = plantList.plantList[5];
        cityGrid.GetGridObject(2, 8).ObjectTile = plantList.plantList[5];
        cityGrid.GetGridObject(2, 9).ObjectTile = plantList.plantList[5];
        cityGrid.GetGridObject(8, 2).ObjectTile = plantList.plantList[5];
    }

    void OnTileSelected(object sender, OnSelectionButtonEventArgs e )
    {
        changeSO = e.selectedSO;
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
                if (hmgo != null && changeSO != null)
                {
                    // Check first values before setting
                    bool canBePlaced = changeSO.CanBeBuiltOn(hmgo.BaseTile);
                    canBePlaced &= changeSO.CheckCost();

                    if (changeSO.CanBeBuiltOn(hmgo.BaseTile))
                    {

                        if (changeSO.CheckCost())
                        {
                            Debug.Log("Setting new tile:" + position);
                            hmgo.ObjectTile = changeSO.baseImage;
                            hmgo.buildableSO = changeSO;
                            changeSO.SubtractCost();
                            GameObject.Find("PlaceSound").GetComponent<AudioSource>().Play();
                        }
                        else
                        {
                            Debug.Log("Not enough resources!");
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot be placed here!");
                    }

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

    public void TriggerSelectionButtonClicked(BuildableSO selection)
    {
        if (OnSelectionButtonClicked != null)
        {
            OnSelectionButtonClicked(this, new OnSelectionButtonEventArgs { selectedSO = selection });
        }
    }
}
