using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingAnimal : MonoBehaviour
{

    Tilemap objectGrid;
    GridManagement grid;

    Vector3 gridDirectionX = new Vector3(1, 0.5f, 0);
    Vector3 gridDirectionY = new Vector3(1, -0.5f, 0);
    public float movingError = 0.005f;

    public float movingSpeed = 0.1f;

    public Vector3Int destination;
    public Vector3Int destination2;
    public bool backAndForth = true;
    public bool flipSprite = true;

    public SpawnType spawnType = SpawnType.Bee;

    public Vector3Int focus;

    private Vector3Int startingPosition;

    public GameObject grassGrower;
    public GameObject dumpRemover;
    public GameObject mallRemover;

    // Start is called before the first frame update
    void Start()
    {
        objectGrid = FindObjectOfType<Tilemap>();
        grid  = FindObjectOfType<GridManagement>();
        startingPosition = objectGrid.WorldToCell(transform.position);
        startingPosition.z = 0;

        if (spawnType == SpawnType.Bee)
        {
            BeeBehaviour();
        }

        if (spawnType == SpawnType.Goat)
        {
            GoatBehaviour();
        }

        if (spawnType == SpawnType.Squirrel)
        {
            SquirrelBehaviour();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Mathf.Round(Time.time) != Mathf.Round(Time.time - Time.deltaTime)) { }


        var destPos = objectGrid.CellToWorld(destination);
        var displacement = destPos - transform.position;
        displacement.z = 0;

        if (displacement.magnitude > movingError)
        {
            displacement = displacement.normalized;
            //Debug.Log(displacement);
            transform.position += displacement * movingSpeed * Time.deltaTime;

            if(flipSprite)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = displacement.x > 0;
            }
        }
        else
        {
            transform.position = new Vector3(destPos.x, destPos.y, transform.position.z);
            if (backAndForth)
            {
                var newDestination = destination2;
                destination2 = destination;
                destination = newDestination;
            }
        }

        if (spawnType == SpawnType.Bee)
        {
            BeeBehaviour();
        }

        if (spawnType == SpawnType.Goat)
        {
            GoatBehaviour();
        }

        if (spawnType == SpawnType.Squirrel)
        {
            SquirrelBehaviour();
        }

    }

    void BeeBehaviour()
    {
        if (grid.cityGrid == null) return; // Wait until the map is generated

        if (GetTileTypeAt(focus) != BaseTileType.Soil)
        {
            focus = RandomTileOfType(BaseTileType.Soil);
            destination = focus;
            destination2 = startingPosition;
            //Debug.Log(GetTileTypeAt(focus));
            if (GetTileTypeAt(focus) == BaseTileType.Soil)
            {
                Instantiate(grassGrower, objectGrid.CellToWorld(focus), Quaternion.identity, null);
            }
        }
    }

    void GoatBehaviour()
    {
        if (grid.cityGrid == null) return; // Wait until the map is generated

        var bush = grid.cityGrid.GetGridObject(startingPosition.x, startingPosition.y).PlantTile;
        /*if (Mathf.Round(Time.time) != Mathf.Round(Time.time - Time.deltaTime))
        {
            if (GetGarbageTypeAt(focus) == GarbageTileType.Dumpster)
            {
                grid.cityGrid.GetGridObject(focus.x, focus.y).Resource.SubtractResource(bush.resourceOut[0].count);
            }
        }*/

        if (!bush.CheckUpkeep())
        {
            destination = startingPosition;
            destination = startingPosition;
        }

        if (bush.CheckUpkeep() && GetGarbageTypeAt(focus) != GarbageTileType.Dumpster)
        {
            focus = RandomResourceOfType(GarbageTileType.Dumpster);
            destination = focus;
            destination2 = startingPosition;
            //Debug.Log(GetTileTypeAt(focus));
            if (GetGarbageTypeAt(focus) == GarbageTileType.Dumpster)
            {
                if (dumpRemover)
                    Instantiate(dumpRemover, objectGrid.CellToWorld(focus), Quaternion.identity, null);
            }
        }
    }
    void SquirrelBehaviour()
    {
        if (grid.cityGrid == null) return; // Wait until the map is generated

        var bush = grid.cityGrid.GetGridObject(startingPosition.x, startingPosition.y).PlantTile;
        /*if (Mathf.Round(Time.time) != Mathf.Round(Time.time - Time.deltaTime))
        {
            if (GetGarbageTypeAt(focus) == GarbageTileType.Mall)
            {
                grid.cityGrid.GetGridObject(focus.x, focus.y).Resource.SubtractResource(bush.resourceOut[0].count);
            }
        }*/

        if (!bush.CheckUpkeep())
        {
            destination = startingPosition;
            destination = startingPosition;
        }

        if (bush.CheckUpkeep() && GetGarbageTypeAt(focus) != GarbageTileType.Mall)
        {
            focus = RandomResourceOfType(GarbageTileType.Mall);
            destination = focus;
            destination2 = startingPosition;
            //Debug.Log(GetTileTypeAt(focus));
            if (GetGarbageTypeAt(focus) == GarbageTileType.Mall)
            {
                if (mallRemover)
                    Instantiate(mallRemover, objectGrid.CellToWorld(focus), Quaternion.identity, null);
            }
        }
    }

    BaseTileType GetTileTypeAt(Vector3Int v)
    {
        if (!grid.cityGrid.GetGridObject(v.x, v.y).BaseTile) return BaseTileType.None;
        return GroundDictionary.Instance.GetTileType(grid.cityGrid.GetGridObject(v.x, v.y).BaseTile);
    }
    GarbageTileType GetGarbageTypeAt(Vector3Int v)
    {
        if (!grid.cityGrid.GetGridObject(v.x, v.y).Resource) return GarbageTileType.None;
        return GarbageDictionary.Instance.GetTileType(grid.cityGrid.GetGridObject(v.x, v.y).Resource);
    }

    Vector3Int RandomTileOfType(BaseTileType type)
    {
        List<CityMapGridObject> objects = new List<CityMapGridObject>();
        // Tally up all used and produced resources
        for (int x = 0; x < grid.cityGrid.Width; x++)
        {
            for (int y = 0; y < grid.cityGrid.Height; y++)
            {
                var obj = grid.cityGrid.GetGridObject(x, y);
                if (obj != null && obj.BaseTile != null)
                {
                    if (GroundDictionary.Instance.GetTileType(obj.BaseTile) == type)
                    {
                        objects.Add(obj);
                    }
                }
            }
        }
        if (objects.Count == 0) return startingPosition;
        var rand = objects[Random.Range(0, objects.Count - 1)];
        return new Vector3Int(rand.x, rand.y, 0);
    }

    Vector3Int RandomResourceOfType(GarbageTileType type)
    {
        List<CityMapGridObject> objects = new List<CityMapGridObject>();
        // Tally up all used and produced resources
        for (int x = 0; x < grid.cityGrid.Width; x++)
        {
            for (int y = 0; y < grid.cityGrid.Height; y++)
            {
                var obj = grid.cityGrid.GetGridObject(x, y);
                if (obj != null && obj.Resource != null)
                {
                    if (GarbageDictionary.Instance.GetTileType(obj.Resource) == type)
                    {
                        objects.Add(obj);
                    }
                }
            }
        }
        if (objects.Count == 0) return startingPosition;
        var rand = objects[Random.Range(0, objects.Count - 1)];
        return new Vector3Int(rand.x, rand.y, 0);
    }


    Vector3Int CellCoordinates(Vector3 pos)
    {
        return objectGrid.WorldToCell(pos);
    }

    Vector3 RoundCoordinates(Vector3 pos)
    {
        var cell = objectGrid.WorldToCell(pos);
        return objectGrid.CellToWorld(cell);
    }

}
