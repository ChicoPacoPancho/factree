using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public static MessagePanel Instance;

    public float disappearTimer = 4;
    public float disappearRate = 0.999f;
    public bool disappearing = false;

    private Text messageText;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => OnDismiss());
        messageText = transform.GetChild(0).GetComponentInChildren<Text>();
        Invoke("Disappear", disappearTimer);
    }


    // Update is called once per frame
    void Update()
    {
        if (disappearing)
        {
            var color = messageText.color;
            color.a *= disappearRate;
            messageText.color = color;
        }
    }

    public void ShowMessage(string text)
    {
        disappearing = false;
        CancelInvoke("Disappear");
        transform.GetChild(0).GetComponentInChildren<Text>().text = text;
        transform.GetChild(0).GetComponentInChildren<Text>().color = Color.white;
        transform.GetChild(0).gameObject.SetActive(true);
        Invoke("Disappear", disappearTimer);
    }

    public void Disappear()
    {
        disappearing = true;
    }

    public void OnDismiss()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }


}
