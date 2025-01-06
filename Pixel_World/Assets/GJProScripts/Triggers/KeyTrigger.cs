using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ô¿³×´¥·¢
public class KeyTrigger : MonoBehaviour
{

    public TipPanel tip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tip.SetTip("Ô¿³×,ÒÑÊ°È¡");

            PlayerData.GetInstance().m_IsGetKey = true;
            GameObject.Destroy(gameObject);
        }
    }
}
