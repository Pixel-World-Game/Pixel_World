using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Կ�״���
public class KeyTrigger : MonoBehaviour
{

    public TipPanel tip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tip.SetTip("Կ��,��ʰȡ");

            PlayerData.GetInstance().m_IsGetKey = true;
            GameObject.Destroy(gameObject);
        }
    }
}
