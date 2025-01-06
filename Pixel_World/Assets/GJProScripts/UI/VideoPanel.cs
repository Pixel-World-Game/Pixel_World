using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPanel : MonoBehaviour
{
    private float m_CurTime;

    public float TTime;

    public AudioSource AS;

    private void OnEnable()
    {
        if (AS != null) 
            AS.Stop();
    }

    private void OnDisable()
    {
        if (AS != null)
            AS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        m_CurTime += Time.deltaTime;
        if(m_CurTime>=TTime)
        {
            if(SceneManager.GetActiveScene().name == "Menu")
            {
                SceneManager.LoadScene("Game1");
                m_CurTime = 0;
            }

            if (SceneManager.GetActiveScene().name == "Game3")
            {
                SceneManager.LoadScene("Menu");
                m_CurTime = 0;
            }
        }
    }
}
