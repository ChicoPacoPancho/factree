using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    public List<Card> cards;
    public BuildableSO[] scriptableObjects;
    public GameObject prefab;

    private TooltipPanel tooltipPanel;
    private bool scrollToTop = false;

    public Sprite noneSprite;

    // Start is called before the first frame update
    void Start()
    {
        tooltipPanel = FindObjectOfType<TooltipPanel>();
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
                card.label = card.scriptableObj.GetDisplayName();
                card.tile = card.scriptableObj.baseImage;
            }
            var sprite = noneSprite;
            if (card.tile) sprite = card.tile.sprite;

            newItem.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            newItem.transform.Find("Label").GetComponent<Text>().text = card.label;
            newItem.transform.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonClick(card));

            var trigger = newItem.transform.GetComponentInChildren<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { OnPointerEnter(card); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => { OnPointerExit(card); });
            trigger.triggers.Add(entry);

            // Pass down events for scrolling to the scroll view
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Scroll;
            entry.callback.AddListener((eventData) => { GetComponentInParent<Image>().SendMessage("OnScroll", eventData); }); 
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((eventData) => { GetComponentInParent<Image>().SendMessage("OnBeginDrag", eventData);  });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((eventData) => { GetComponentInParent<Image>().SendMessage("OnDrag", eventData); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((eventData) => { GetComponentInParent<Image>().SendMessage("OnEndDrag", eventData); });
            trigger.triggers.Add(entry);

            //newItem.transform.Find("Number").GetComponent<Text>().text = "" + r.quanitity;
            card.gameObject = newItem;
        }

        transform.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        //transform.GetChild(0).GetComponent<Button>().Select();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        int i;

        for (i = 0; i < cards.Count; i++)
        {
            if (cards[i].scriptableObj.CheckCost() || FindObjectOfType<GridManagement>().cheatMode)
            {
                cards[i].gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                cards[i].gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnButtonClick(Card card)
    {
        //Debug.Log(card.label + " selected");
        MessagePanel.Instance.ShowMessage(card.label + " selected");

        GetComponent<AudioSource>().Play();
               
        var GM = FindObjectOfType<GridManagement>();
        GM.TriggerSelectionButtonClicked(card.scriptableObj);
    }

    public void OnPointerEnter(Card card)
    {
        tooltipPanel.SetData(card.scriptableObj);
    }

    public void OnPointerExit(Card card)
    {
        tooltipPanel.HidePanel();
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

