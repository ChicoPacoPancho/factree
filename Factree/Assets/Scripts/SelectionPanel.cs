using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    public List<Card> cards;
    public BuildableSO[] scriptableObjects;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BuildableSO scriptable in scriptableObjects)
        {
            var card = new Card();
            card.scriptableObj = scriptable;
            cards.Add(card);
        }
        foreach (Card card in cards)
        {
            var newItem = Instantiate(prefab, transform);
            if (card.scriptableObj != null)
            {
                card.label = SplitCamelCase(card.scriptableObj.name);
                card.tile = card.scriptableObj.baseImage;
            }
            newItem.transform.Find("Image").GetComponent<Image>().sprite = card.tile.sprite;
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
            Debug.Log(card.label + " was chosen");

            var GM = FindObjectOfType<GridManagement>();
            GM.TriggerSelectionButtonClicked(card.tile);
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

}

[Serializable]
public class Card
{
    public string label;
    public Sprite sprite;
    public Tile tile;
    public BuildableSO scriptableObj; 
    public GameObject gameObject;
}

