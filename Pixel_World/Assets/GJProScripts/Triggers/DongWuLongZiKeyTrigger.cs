using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongWuLongZiKeyTrigger : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
         GameObject.Find("Canvas111").GetComponent<UIMgr>().TipUI.GetComponent<TipPanel> ().SetTip("The keys have been picked up");

            PlayerData.GetInstance().m_IsGetLongZiKey = true;

            GameObject.Destroy(gameObject);
        } 
    }
}
