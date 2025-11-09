using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicToMemorySphere : MonoBehaviour
{    

    private void Start()
    {
        AudioManager.instance.ChangeActiveTrackParameters(0, 0, 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.instance.ChangeActiveTrackParameters(0, 0, 1);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.instance.ChangeActiveTrackParameters(1, 0, 0);
        }
    }
}
