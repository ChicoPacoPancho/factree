using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float movingSpeed = 0.1f;

    public bool changeVolumeWithDistance = true;
    public float maxVolume = 0.1f;
    public Vector3 mapCenter = Vector3.zero;
    public float maxVolumeDistance = 3;
    public float fadeOutDistance = 7;
    public float volumeChangeSpeed = 0.01f;

    AudioSource musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.Find("MusicPlayer");
        if (obj != null)
            musicPlayer = obj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        if (horiz > 0)
        {
            transform.position += movingSpeed * Vector3.right;
        }
        else if(horiz < 0)
        {
            transform.position += movingSpeed * Vector3.left;
        }

        if (vert > 0)
        {
            transform.position += movingSpeed * Vector3.up;
        }
        else if (vert < 0)
        {
            transform.position += movingSpeed * Vector3.down;
        }

        if (musicPlayer && changeVolumeWithDistance)
        {
            var pos = transform.position;
            pos.z = 0;
            var distanceFromCenter = (mapCenter - pos).magnitude;
            var fraction = 1f;
            if (distanceFromCenter > maxVolumeDistance)
            {
                if (distanceFromCenter < fadeOutDistance)
                {
                    fraction = 1 - ((distanceFromCenter - maxVolumeDistance) / (fadeOutDistance - maxVolumeDistance));
                } else
                {
                    fraction = 0;
                }
            }
            //Debug.Log(distanceFromCenter + " " + fraction);

            var desiredVolume = maxVolume * fraction;

            if (Mathf.Abs(musicPlayer.volume - desiredVolume) < volumeChangeSpeed)
            {
                musicPlayer.volume = desiredVolume;
            }
            else
            {
                musicPlayer.volume += volumeChangeSpeed * Mathf.Sign(desiredVolume - musicPlayer.volume);
            }
        }
    }



}
