using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroySelfTimer : MonoBehaviour
{

    public float delay = 5;
    public GameObject activateOnDestroy = null;
    public string playOnDestroy = "";

    public bool setBaseTile = false;
    public Tile baseTile;
    public bool setObjectTile = false;
    public Tile objectTile;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TimerUp", delay);
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
        if (activateOnDestroy)
        {
            activateOnDestroy.SetActive(true);
        }
        var pos = gameObject.transform.position;
        pos.z = 0;
        Vector3Int gridXY = FindObjectOfType<Tilemap>().WorldToCell(pos);
        Debug.Log(gridXY);
        if (setBaseTile)
        {
            FindObjectOfType<GridManagement>().cityGrid.GetGridObject(gridXY.x, gridXY.y).BaseTile = baseTile;
        }
        if (setObjectTile)
        { 
            FindObjectOfType<GridManagement>().cityGrid.GetGridObject(gridXY.x, gridXY.y).ObjectTile = objectTile;
        }
        Destroy(gameObject);
    }


}
