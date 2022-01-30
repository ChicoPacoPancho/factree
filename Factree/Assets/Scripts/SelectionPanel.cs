using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    public Card[] cards;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Card card in cards)
        {
            var newItem = Instantiate(prefab, transform);
            newItem.transform.Find("Image").GetComponent<Image>().sprite = card.sprite;
            newItem.transform.Find("Label").GetComponent<Text>().text = card.label;
            newItem.transform.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonClick(card));
            //newItem.transform.Find("Number").GetComponent<Text>().text = "" + r.quanitity;
            card.gameObject = newItem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick(Card card)
    {
        Debug.Log( card.label + " was chosen");

        var GM = FindObjectOfType<GridManagement>();
        GM.TriggerSelectionButtonClicked(card.sprite);
    }

}



[Serializable]
public class Card
{
    public string label;
    public Sprite sprite;
    public GameObject gameObject;
}

