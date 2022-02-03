using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TooltipPanel : MonoBehaviour
{

    public BuildableSO setObject;
    public GarbageSO setRObject;

    public GameObject prefab;

    public Sprite noneSprite;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePanel();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePanel()
    {
        var subpanel = transform.GetChild(0);

        // Clear panels first
        string[] panelsToClear = { "Cost Panel", "Production Panel", "Upkeep Panel", "Contains Panel" };

        foreach (string panel in panelsToClear)
        {
            var section = subpanel.Find("Resources").Find(panel);
            foreach (Transform child in section)
            {
                if (child.name != "Label") Destroy(child.gameObject);
            }
            section.gameObject.SetActive(false);
        }

        if (setObject == null && setRObject == null) return;

        var card = subpanel.Find("Card");

        
        string label = "";
        if (setObject != null) label = setObject.name;
        if (setRObject != null) label = setRObject.name;

        Sprite sprite = noneSprite;
        if (setObject != null && setObject.baseImage != null) sprite = setObject.baseImage.sprite;
        if (setRObject != null && setRObject.baseImage != null) sprite = setRObject.baseImage.sprite;

        card.GetComponentInChildren<Text>().text = SelectionPanel.SplitCamelCase(label);
        card.GetComponentInChildren<Image>().sprite = sprite;

        Transform resourceSidePanel = subpanel.Find("Resources");

        if (setObject != null)
        {
            Populate(resourceSidePanel.Find("Production Panel"), setObject.resourceOut, ProductionFormat);
            
            Populate(resourceSidePanel.Find("Upkeep Panel"), setObject.resourceIn, UpkeepFormat);

            Populate(resourceSidePanel.Find("Cost Panel"), setObject.builtCost, CostFormat);
        }

        if (setRObject != null)
        {
            Populate(resourceSidePanel.Find("Contains Panel"), new List<ResourceItem> { setRObject.resource }, ContainsFormat);
        }
    }

    void Populate(Transform panel, List<ResourceItem> items, Func<float, string> NumberFormat)
    {
        foreach (var r in items)
        {
            var newItem = Instantiate(prefab, panel.transform);
            newItem.transform.Find("Image").GetComponent<Image>().sprite = ResourceDictionary.Instance.GetSprite(r.resourceType);
            var textObject = newItem.transform.Find("Number").GetComponent<Text>();
            textObject.text = NumberFormat(r.count);
        }
        panel.gameObject.SetActive(items.Count > 0);
    }


    string ProductionFormat(float n)
    {
        return "+" + Mathf.Round(n / 60f * 10) / 10;
        
    }
    string UpkeepFormat(float n)
    {
        return "-" + Mathf.Round(n / 60f * 10) / 10;
    }
    string CostFormat(float n)
    {
        return "-" + Mathf.Round(n * 10) / 10;
    }
    string ContainsFormat(float n)
    {
        return "" + Mathf.Round(n * 10) / 10;
    }


    public void SetData(BuildableSO so)
    {
        if (so == null)
        {
            HidePanel();
            return;
        }
        var subpanel = transform.GetChild(0);
        setObject = so;
        setRObject = null;
        UpdatePanel();
        subpanel.gameObject.SetActive(true);
    }

    public void SetResourceData(GarbageSO so)
    {
        if (so == null)
        {
            HidePanel();
            return;
        }
        var subpanel = transform.GetChild(0);
        setRObject = so;
        setObject = null;
        UpdatePanel();
        subpanel.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        var subpanel = transform.GetChild(0);
        subpanel.gameObject.SetActive(false);
    }

}
