using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MovingAnimal : MonoBehaviour
{

    protected Tilemap objectGrid;
    protected GridManagement grid;

    Vector3 gridDirectionX = new Vector3(1, 0.5f, 0);
    Vector3 gridDirectionY = new Vector3(1, -0.5f, 0);
    public float movingError = 0.005f;

    public float movingSpeed = 0.1f;

    public bool flipSprite = true;
    private Coroutine movementCoroutine;
    private Coroutine behaviourCoroutine;

    SpriteRenderer childSprite;
    public Sprite idleSprite;
    public Sprite movingSprite;
    public Sprite busySprite;

    public Vector3Int focus;
    protected Vector3Int startingPosition;

    private AnimalState m_currentState = AnimalState.Idle;
    public AnimalState currentState 
    {
        get {
            return m_currentState;
        } 
        protected set {
            if(m_currentState != value)
            {
                m_currentState = value;
                switch (value)
                {
                    case AnimalState.Busy:
                        if (busySprite != null) childSprite.sprite = busySprite;
                        break;
                    case AnimalState.Moving:
                        if (movingSprite != null) childSprite.sprite = movingSprite;
                        break;
                    case AnimalState.Idle:
                    default:
                        if (idleSprite != null) childSprite.sprite = idleSprite;
                        break;
                }
            }
        } 
    }


    void Start()
    {
        objectGrid = FindObjectOfType<Tilemap>();
        grid  = FindObjectOfType<GridManagement>();
        childSprite = GetComponentInChildren<SpriteRenderer>();
        startingPosition = objectGrid.WorldToCell(transform.position);
        startingPosition.z = 0;

        StartBehaviour();
    }

    void Update()
    {
        
        if(currentState == AnimalState.Idle)
        {
            StartBehaviour();
        }
    }

    protected virtual IEnumerator DoMoveToGridPosition(Vector3Int destination, AnimalState stateAfterMove = AnimalState.Idle)
    {
        Debug.Log("DO Move");
        yield return DoMoveToWorldPosition(objectGrid.CellToWorld(destination), stateAfterMove);
    }

    protected virtual IEnumerator DoMoveToWorldPosition(Vector3 destPos, AnimalState stateAfterMove = AnimalState.Idle)
    {
        Vector3 displacement = destPos - transform.position;
        displacement.z = 0;
        Vector3 step;
        currentState = AnimalState.Moving;

        while (displacement.magnitude > movingError)
        {
            displacement = destPos - transform.position;
            displacement.z = 0;
            step = displacement.normalized;
            //Debug.Log(displacement);
            transform.position += step * movingSpeed * Time.deltaTime;

            if (flipSprite)
            {
                childSprite.flipX = displacement.x > 0;
            }

            yield return null;
        }

        currentState = stateAfterMove;
    }


    void StartBehaviour()
    {
        if(behaviourCoroutine != null)
            StopCoroutine(behaviourCoroutine);
        behaviourCoroutine = StartCoroutine(DoBehaviour());
    }

    protected virtual IEnumerator DoBehaviour()
    {
        currentState = AnimalState.Busy;
        return null;
    }

    protected BaseTileType GetTileTypeAt(Vector3Int v)
    {
        if (!grid.cityGrid.GetGridObject(v.x, v.y).BaseTile) return BaseTileType.None;
        return GroundDictionary.Instance.GetTileType(grid.cityGrid.GetGridObject(v.x, v.y).BaseTile);
    }
    protected GarbageTileType GetGarbageTypeAt(Vector3Int v)
    {
        if (!grid.cityGrid.GetGridObject(v.x, v.y).Resource) return GarbageTileType.None;
        return GarbageDictionary.Instance.GetTileType(grid.cityGrid.GetGridObject(v.x, v.y).Resource);
    }

    protected Vector3Int RandomTileOfType(BaseTileType type)
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

    protected Vector3Int RandomResourceOfType(GarbageTileType type)
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

    public enum AnimalState
    {
        Idle,
        Moving,
        Busy
    }
}
