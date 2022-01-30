using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    public AudioClip successMusic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowVictory()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("MusicPlayer").GetComponent<AudioSource>().clip = successMusic;
        GameObject.Find("MusicPlayer").GetComponent<AudioSource>().Play();

    }
    public void HideVictory()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }


}
