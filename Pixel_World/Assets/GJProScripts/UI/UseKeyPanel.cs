using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UseKeyPanel : MonoBehaviour
{
    public Button m_OpenDorr;
    
    void Start()
    {
        m_OpenDorr.onClick.AddListener(() =>
        {
            PlayerData.GetInstance().m_IsOpen = true;
        });
    }

    
}
