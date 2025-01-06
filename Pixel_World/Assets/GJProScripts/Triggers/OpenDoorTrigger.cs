using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ŵ�����
public class OpenDoorTrigger : MonoBehaviour
{
    public GameObject UseKeyUI;

    public TipPanel tip;

    

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerData.GetInstance().m_IsOpen)
            return;
        if (other.tag == "Player")
        {
            if (PlayerData.GetInstance().m_IsNpcDuiHua)
            { 
                UseKeyUI.SetActive(true);
               
            } 
            else
            {
                tip.SetTip("û�������޷���");
            }    
        }
    }
}
