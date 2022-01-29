using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickBehaviour : MonoBehaviour
{

    public Tilemap grid;
    public Tile setTo;
    public GameObject follow;
    public GameObject selectionSquare;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the mouse and convert it to cells
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0; // By default the function above returns -10 which is not right
        Vector3Int cell = grid.WorldToCell(pos);

        Vector3 roundedPos = grid.CellToWorld(cell);
        selectionSquare.transform.position = roundedPos;

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
