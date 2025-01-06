using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{ 
    void Start()
    {
        float v = PlayerPrefs.GetFloat("M");
        GetComponent<AudioSource>().volume = v;
    } 
}
