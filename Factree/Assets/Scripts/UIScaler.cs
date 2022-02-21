using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    /*
     * Takes in the current scale from the pixel
     * perfect camera and applies it to the 
     * canvas scale as well.
     */

    public float UIRatio = 0.5f;
    public bool roundDown = true;

    PixelPerfectCamera pixelCamera;
    CanvasScaler canvas;


    // Start is called before the first frame update
    void Start()
    {
        pixelCamera = FindObjectOfType<PixelPerfectCamera>();
        canvas = FindObjectOfType<CanvasScaler>();
    }

    // Has to be LateUpdate because pixel perfect camera updates in Update
    void LateUpdate()
    {
        float newScale = pixelCamera.pixelRatio * UIRatio;
        if(roundDown && newScale > 1) newScale = Mathf.Floor(newScale);
        canvas.scaleFactor = newScale;
    }
}
