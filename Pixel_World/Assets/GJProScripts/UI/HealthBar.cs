using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public PlayerStats state;

    public Image hpbar; 

    void Update()
    {
        transform.forward = Camera.main.transform.forward;

        if(hpbar!=null)
        {
            hpbar.fillAmount = (float)state.m_CurHp / state.m_MaxHp;
        }
    }
}
