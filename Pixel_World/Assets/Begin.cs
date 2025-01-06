using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Begin : MonoBehaviour {

    public GameObject gameObject1;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Next(int i)
    {
        SceneManager.LoadScene(i);
    }
    public void Exit()
    {
        Application.Quit();
    }
  
}
