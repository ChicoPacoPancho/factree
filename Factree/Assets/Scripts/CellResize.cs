using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellResize : MonoBehaviour
{
    public int widthCount;
    public int heightCount;
    
    public GameObject container;
    public GameObject template;
    // Start is called before the first frame update
    void Start()
    {
        float width = container.GetComponent<RectTransform>().rect.width - 1;
        float height = container.GetComponent<RectTransform>().rect.height - 1;
        Vector2 newSize = new Vector2(width / widthCount, width / widthCount);

        for(int i = 0;i<widthCount*heightCount; i++)
        {
            var temp = Instantiate(template, transform);
            temp.GetComponent<DisplayAmount>().SetAmount(0);
        }

        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
        RectTransform rt = container.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, heightCount * newSize.y);

    
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
