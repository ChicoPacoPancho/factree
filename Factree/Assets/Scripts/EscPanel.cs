using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPanel : MonoBehaviour
{

    bool visible = false;
    public GameObject gameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        visible = !visible;
        gameObject.SetActive(visible);
    }

    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        ToggleMenu();
#endif
        Application.Quit();
    }


    public void OnResumeButtonClick()
    {
        ToggleMenu();
    }

}
