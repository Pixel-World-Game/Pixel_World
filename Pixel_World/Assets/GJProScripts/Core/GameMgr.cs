using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//整个游戏管理器
public class GameMgr : MonoBehaviour
{
    //剩余时间
    public float m_LeftTime;

    public bool m_IsStart;

    public GameObject GameOverUI;

    void Update()
    {
        if(m_IsStart)
        {
            m_LeftTime -= Time.deltaTime;
            if (m_LeftTime <= 0)
            {
                Debug.Log("游戏结束");
                GameOverUI.SetActive(true);
            }
        } 
    }
}
