using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingAnimal : MonoBehaviour
{

    Tilemap objectGrid;

    Vector3 gridDirectionX = new Vector3(1, 0.5f, 0);
    Vector3 gridDirectionY = new Vector3(1, -0.5f, 0);
    public float movingError = 0.005f;

    public float movingSpeed = 0.1f;

    public Vector3Int destination;
    public Vector3Int destination2;
    public bool backAndForth = true;
    public bool flipSprite = true;

    // Start is called before the first frame update
    void Start()
    {
        objectGrid = FindObjectOfType<Tilemap>();
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
            Debug.Log(displacement);
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
