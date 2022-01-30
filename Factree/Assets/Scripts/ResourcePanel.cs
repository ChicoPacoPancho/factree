using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    public ResourceManager resourceManager;
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
            r.textObject = newItem.transform.Find("Number").GetComponent<Text>();
            r.textObject.text = "" + SpecialRound(resourceManager.GetResourceAmountByType(r.type));
            r.gameObject = newItem;
        }

        resourceManager.OnResourceChanged += ResourceManager_OnResourceChanged;

        transform.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
    }

    private void ResourceManager_OnResourceChanged(object sender, ResourceManager.ResourceChangedEventArgs e)
    {
        foreach (Resource r in resources)
        {
            if (r.type == e.type)
            {
                r.textObject.text = "" + SpecialRound(e.newAmount);
            }
        }
    }

    float SpecialRound(float a)
    {
        return Mathf.Floor(a);
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
    public ResourceType type;
    public int quanitity;
    public Sprite sprite;
    public GameObject gameObject;
    public Text textObject;
}

