using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    public Resource[] resources;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Resource r in resources)
        {
            var newItem = Instantiate(prefab, transform);
            newItem.transform.Find("Image").GetComponent<Image>().sprite = r.sprite;
            newItem.transform.Find("Label").GetComponent<Text>().text = r.label;
            newItem.transform.Find("Number").GetComponent<Text>().text = "" + r.quanitity;
            r.gameObject = newItem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuantity(string resourceId, int quantity)
    {
        // TODO
    }
}

[Serializable]
public class Resource
{
    public string label;
    public int quanitity;
    public Sprite sprite;
    public GameObject gameObject;
}

