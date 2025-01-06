using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//结束触发器
public class GameOverTrigger : MonoBehaviour
{
    //结束页面
    public GameObject GameOverUI;

    public TipPanel tip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //如果拿到钥匙
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
