using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    int sortingOrderBase = 20;
    public int offset = 0;
    public bool runOnlyOnce = false;

    float timer;
    float timerMax = .1f;
    public SpriteRenderer myRenderer;


    void Awake()
    {
          myRenderer = gameObject.GetComponent<SpriteRenderer>();
          //myRenderer.sprite = item.picture;
    }


    void LateUpdate()
    {
        if (myRenderer != null)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                myRenderer.sortingOrder = (int)(sortingOrderBase - (1 * transform.position.y) - offset);
                timer = timerMax;
            }

            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
