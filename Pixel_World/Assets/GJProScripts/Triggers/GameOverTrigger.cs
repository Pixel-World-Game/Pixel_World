using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������
public class GameOverTrigger : MonoBehaviour
{
    //����ҳ��
    public GameObject GameOverUI;

    public TipPanel tip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //����õ�Կ��
            if(PlayerData.GetInstance().m_IsGetKey)
            {
                GameOverUI.SetActive(true);
            }
            else
            {
                tip.SetTip("I haven't got the keys yet. Try again");
            } 
        }
    }
}
