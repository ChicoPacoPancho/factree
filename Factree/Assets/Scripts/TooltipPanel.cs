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
        string[] panelsToClear = { "Cost Panel", "Production Panel", "Upkeep Panel" };

        foreach (string panel in panelsToClear)
        {
            foreach (Transform child in subpanel.Find(panel))
            {
                if (child.name != "Label") Destroy(child.gameObject);
            }
        }

        if (setObject == null && setRObject == null) return;

        var card = subpanel.Find("Card");

        
        string label = "";
        if (setObject != null) label = setObject.name;
        if (setRObject != null) label = setRObject.name;

        Sprite sprite = null;
        if (setObject != null) sprite = setObject.baseImage.sprite;
        if (setRObject != null) sprite = setRObject.baseImage.sprite;

        card.GetComponentInChildren<Text>().text = SelectionPanel.SplitCamelCase(label);
        card.GetComponentInChildren<Image>().sprite = sprite;

        if (setObject != null)
        {
            foreach (var r in setObject.resourceOut)
            {
                var newItem = Instantiate(prefab, subpanel.Find("Production Panel"));
                newItem.transform.Find("Image").GetComponent<Image>().sprite = ResourceDictionary.Instance.GetSprite(r.resourceType);
                var textObject = newItem.transform.Find("Number").GetComponent<Text>();
                textObject.text = "+" + SpecialRound(r.count);
            }

            foreach (var r in setObject.resourceIn)
            {
                var newItem = Instantiate(prefab, subpanel.Find("Upkeep Panel"));
                newItem.transform.Find("Image").GetComponent<Image>().sprite = ResourceDictionary.Instance.GetSprite(r.resourceType);
                var textObject = newItem.transform.Find("Number").GetComponent<Text>();
                textObject.text = "-" + SpecialRound(r.count);
            }

            foreach (var r in setObject.builtCost)
            {
                var newItem = Instantiate(prefab, subpanel.Find("Cost Panel"));
                newItem.transform.Find("Image").GetComponent<Image>().sprite = ResourceDictionary.Instance.GetSprite(r.resourceType);
                var textObject = newItem.transform.Find("Number").GetComponent<Text>();
                textObject.text = "-" + r.count;
            }
        }

        if (setRObject != null)
        {
            var newItem = Instantiate(prefab, subpanel.Find("Production Panel"));
            newItem.transform.Find("Image").GetComponent<Image>().sprite = ResourceDictionary.Instance.GetSprite(setRObject.resource.resourceType);
            var textObject = newItem.transform.Find("Number").GetComponent<Text>();
            textObject.text = "+" + SpecialRound(setRObject.resource.count*60);
        }
    }

    float SpecialRound(float a)
    {
        return Mathf.Round(a /60f * 10) / 10;
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
