using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletSoundImpactRand : MonoBehaviour
{
    public AudioClip s1, s2;

    void Awake()
    {
        if (Random.Range(0, 100) % 2 == 0)
        {
            GetComponent<AudioSource>().clip = s1;
        } else
            {
                GetComponent<AudioSource>().clip = s2;
            }
    }

    void Start()
    {
        GetComponent<AudioSource>().Play();
    }
}//EndScript