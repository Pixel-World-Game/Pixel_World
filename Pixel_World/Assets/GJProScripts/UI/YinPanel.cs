using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class YinPanel : MonoBehaviour
{
    public Scrollbar m_Scrobal;

    public AudioSource As;

    public Button OKBTN;

    public GameObject MainUI;
     
    void Start()
    {
        OKBTN.onClick.AddListener(() =>
        { 
            PlayerPrefs.SetFloat("M", As.volume);

            gameObject.SetActive(false);

            MainUI.SetActive(true);
        });
    }

    private void OnEnable()
    {
        As.volume = PlayerPrefs.GetFloat("M");
    }


    void Update()
    {
        if (As != null)
            As.volume = m_Scrobal.value;
    }
}
