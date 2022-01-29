using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickBehaviour : MonoBehaviour
{

    public Tilemap grid;
    public Tile setTo;
    public GameObject follow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            follow.transform.position = pos; 
            Vector3Int cell = grid.WorldToCell(pos);
            Debug.Log("Position: " + cell);
            Debug.Log(grid.GetTile(cell));

            grid.SetTile(cell, setTo);
            
            grid.RefreshAllTiles();
        }
    }
}
