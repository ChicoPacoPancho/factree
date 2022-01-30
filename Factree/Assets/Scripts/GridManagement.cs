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


    CityMapGridObject selectedObject;
    [SerializeField] BuildableSO changeSO;
    [SerializeField] GarbageSO garbageSO;
    [SerializeField] Tile selectedTile;
    [SerializeField] Tilemap cityMap;
    [SerializeField] GameObject selectionSquare;

    public bool cheatMode = false;
   

    // Start is called before the first frame update
    void Start()
    {
        int j, i;
        this.OnSelectionButtonClicked += OnTileSelected;

        cityGrid = new CityGrid<CityMapGridObject>(12, 12, 1f, Vector3.zero, (CityGrid<CityMapGridObject> g, int x, int y) => new CityMapGridObject(g, x, y));
        
        cityVisual.SetGrid(cityGrid);

        selectedTile = GroundDictionary.Instance.GetTile(BaseTileType.Concrete);

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
                    hmgo.BaseTile = selectedTile;
                }
            }

        // Build up base tiles
        // Road
        selectedTile = GroundDictionary.Instance.GetTile(BaseTileType.Asphalt);
        for (i=3; i<=4; i++)
            for(j=0;j<=10;j++)            
                cityGrid.GetGridObject(i, j).BaseTile = selectedTile;
        // Soil
        selectedTile = GroundDictionary.Instance.GetTile(BaseTileType.Soil);
        for (i = 7; i <= 9; i++)
            for (j = 3; j <= 5; j++)
                cityGrid.GetGridObject(i, j).BaseTile = selectedTile;

        // World Tree
        selectedTile = GroundDictionary.Instance.GetTile(BaseTileType.Grass);
        cityGrid.GetGridObject(8, 4).BaseTile = selectedTile;
        cityGrid.GetGridObject(7, 4).BaseTile = selectedTile;
        cityGrid.GetGridObject(9, 4).BaseTile = selectedTile;
        cityGrid.GetGridObject(8, 3).BaseTile = selectedTile;
        cityGrid.GetGridObject(8, 5).BaseTile = selectedTile;

        // Water
        selectedTile = GroundDictionary.Instance.GetTile(BaseTileType.Water);
        for (j = 3; j <= 7; j++)
            cityGrid.GetGridObject(10, j).BaseTile = selectedTile;
        cityGrid.GetGridObject(9, 2).BaseTile = selectedTile;
        cityGrid.GetGridObject(8, 1).BaseTile = selectedTile;
        cityGrid.GetGridObject(7, 0).BaseTile = selectedTile;



        // Build up Objects
        // World Tree
        changeSO = BuildableDictionary.Instance.GetTile(BuildableTileType.TheGreatTree);
        cityGrid.GetGridObject(8, 4).PlantTile = changeSO;

        // Dumpster
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.Dumpster);
        cityGrid.GetGridObject(8, 6).Resource = garbageSO;
        cityGrid.GetGridObject(8, 5).Resource = garbageSO;
        cityGrid.GetGridObject(2, 6).Resource = garbageSO;
        cityGrid.GetGridObject(2, 5).Resource = garbageSO;

        // Observatory
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.Observatory);
        cityGrid.GetGridObject(5, 2).Resource = garbageSO;
        cityGrid.GetGridObject(7, 5).Resource = garbageSO;
        cityGrid.GetGridObject(1, 7).Resource = garbageSO;
        cityGrid.GetGridObject(1, 6).Resource = garbageSO;
        cityGrid.GetGridObject(0, 5).Resource = garbageSO;

        // Mall
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.Mall);
        cityGrid.GetGridObject(1, 3).Resource = garbageSO;
        cityGrid.GetGridObject(1, 4).Resource = garbageSO;
        cityGrid.GetGridObject(9, 5).Resource = garbageSO;
        cityGrid.GetGridObject(7, 3).Resource = garbageSO;

        // Industry
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.Industry);
        cityGrid.GetGridObject(8, 9).Resource = garbageSO;
        cityGrid.GetGridObject(5, 9).Resource = garbageSO;
        cityGrid.GetGridObject(8, 7).Resource = garbageSO;
        cityGrid.GetGridObject(6, 1).Resource = garbageSO;
        cityGrid.GetGridObject(6, 0).Resource = garbageSO;
        cityGrid.GetGridObject(5, 0).Resource = garbageSO;
        cityGrid.GetGridObject(5, 1).Resource = garbageSO;

        // Car
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.Car);
        cityGrid.GetGridObject(7, 9).Resource = garbageSO;
        cityGrid.GetGridObject(7, 8).Resource = garbageSO;
        cityGrid.GetGridObject(6, 8).Resource = garbageSO;
        cityGrid.GetGridObject(5, 8).Resource = garbageSO;
        cityGrid.GetGridObject(6, 6).Resource = garbageSO;        
        cityGrid.GetGridObject(6, 5).Resource = garbageSO;
        cityGrid.GetGridObject(4, 6).Resource = garbageSO;

        // Skyscraper
        garbageSO = GarbageDictionary.Instance.GetTile(GarbageTileType.SkyScraper);
        cityGrid.GetGridObject(2, 7).Resource = garbageSO;
        cityGrid.GetGridObject(2, 8).Resource = garbageSO;
        cityGrid.GetGridObject(2, 9).Resource = garbageSO;
        cityGrid.GetGridObject(8, 2).Resource = garbageSO;
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
        Vector3 roundedPos = cityMap.CellToWorld(tile);

        selectionSquare.SetActive(false);
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            cheatMode = !cheatMode;
            if (cheatMode)
                MessagePanel.Instance.ShowMessage("Cheatmode ON");
            else
                MessagePanel.Instance.ShowMessage("Cheatmode OFF");

        }

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
                    if (changeSO != null)
                    {
                        // Check first values before setting
                        bool canBePlaced = changeSO.CanBeBuiltOn(hmgo.BaseTile);
                        canBePlaced &= changeSO.CheckCost();

                        if (hmgo.Resource == null)
                        {
                            if (hmgo.PlantTile == null || changeSO.removesPlants)
                            {
                                if (changeSO.CanBeBuiltOn(hmgo.BaseTile))
                                {

                                    if (changeSO.CheckCost() || cheatMode)
                                    {
                                        Debug.Log("Setting new tile:" + position);
                                        if (!changeSO.removesPlants)
                                        {
                                            hmgo.PlantTile = changeSO;
                                            changeSO.SubtractCost();
                                            GameObject.Find("PlaceSound").GetComponent<AudioSource>().Play();
                                            // Spawn any immediate spawns
                                            if (changeSO.spawnInterval == 0)
                                            {
                                                changeSO.SpawnSpawns(roundedPos);
                                            }
                                        }
                                        else
                                        {
                                            if (hmgo.PlantTile.name != "TheGreatTree")
                                            {
                                                hmgo.PlantTile = null;
                                                GameObject.Find("DestroySound").GetComponent<AudioSource>().Play();
                                            }
                                            else
                                                MessagePanel.Instance.ShowMessage("The Great Tree refuses to be removed");

                                        }
                                    }
                                    else
                                    {
                                        MessagePanel.Instance.ShowMessage("Not enough resources to plant this");
                                    }
                                }
                                else
                                {
                                    MessagePanel.Instance.ShowMessage("Selected plant cannot grow on this tile");
                                }
                            } else
                            {
                                MessagePanel.Instance.ShowMessage("There is already a plant here");
                            }
                        }
                        else if (selectedObject.PlantTile != null)
                        {
                            selectedObject.PlantTile.DoAbilitiesOnTarget(selectedObject.x, selectedObject.y, hmgo.x, hmgo.y);
                        } else
                        {
                            MessagePanel.Instance.ShowMessage("Use a Fly Trap or a Wrecking Ball to remove ruins first");
                        }
                    }

                }
                selectedObject = hmgo;
            }

            // Get the position of the mouse and convert it to cells
            if (hmgo.BaseTile != null)
            {
                selectionSquare.transform.position = roundedPos;
                if (hmgo.PlantTile)
                    FindObjectOfType<TooltipPanel>().SetData(hmgo.PlantTile);
                else if (hmgo.Resource)
                    FindObjectOfType<TooltipPanel>().SetResourceData(hmgo.Resource);
                else
                    FindObjectOfType<TooltipPanel>().SetData(null);
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
