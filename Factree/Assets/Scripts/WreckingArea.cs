using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WreckingArea : MonoBehaviour
{
    public float delay = 1f;

    GridManagement grid;
    Vector3Int startingPos;
    public string playOnDestroy = "";

    public bool FlyTrap = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManagement>();
        startingPos = FindObjectOfType<Tilemap>().WorldToCell(transform.position);
        startingPos.z = 0;
        InvokeRepeating("TimerUp", delay, delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimerUp()
    {
        if (playOnDestroy != "")
        {
            GameObject.Find(playOnDestroy).GetComponent<AudioSource>().Play();
        }
        //Debug.Log("Wreck timer");
        var objects = RadiusResources(1);
        if (FlyTrap) objects = RadiusFlyTrapResources(1);

        if (objects.Count > 0)
        {
            var rand = objects[Random.Range(0, objects.Count - 1)];
            ResourceManager.Instance.AddResourceAmountByType(rand.Resource.resource.resourceType, rand.Resource.resource.count);
            rand.Resource = null;
        } else
        {
            Destroy(gameObject);
        }
    }

    List<CityMapGridObject> RadiusResources(int radius)
    {
        List<CityMapGridObject> objects = new List<CityMapGridObject>();
        // Tally up all used and produced resources
        for (int x = Mathf.Max(0, startingPos.x - radius); x <= startingPos.x + radius && x < grid.cityGrid.Width; x++)
        {
            for (int y = Mathf.Max(0, startingPos.y - radius); y <= startingPos.y + radius && y < grid.cityGrid.Width; y++)
            {
                var obj = grid.cityGrid.GetGridObject(x, y);
                if (obj != null && obj.Resource != null)
                {
                    if (GarbageDictionary.Instance.GetTileType(obj.Resource) != GarbageTileType.Car && 
                        GarbageDictionary.Instance.GetTileType(obj.Resource) != GarbageTileType.Dumpster)
                    {
                        objects.Add(obj);
                    }
                }
            }
        }
        return objects;
    }
    List<CityMapGridObject> RadiusFlyTrapResources(int radius)
    {
        List<CityMapGridObject> objects = new List<CityMapGridObject>();
        // Tally up all used and produced resources
        for (int x = Mathf.Max(0, startingPos.x - radius); x <= startingPos.x + radius && x < grid.cityGrid.Width; x++)
        {
            for (int y = Mathf.Max(0, startingPos.y - radius); y <= startingPos.y + radius && y < grid.cityGrid.Width; y++)
            {
                var obj = grid.cityGrid.GetGridObject(x, y);
                if (obj != null && obj.Resource != null)
                {
                    if (GarbageDictionary.Instance.GetTileType(obj.Resource) == GarbageTileType.Car ||
                        GarbageDictionary.Instance.GetTileType(obj.Resource) == GarbageTileType.Dumpster)
                    {
                        objects.Add(obj);
                    }
                }
            }
        }
        return objects;
    }

}
