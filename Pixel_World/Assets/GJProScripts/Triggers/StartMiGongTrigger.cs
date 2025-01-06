using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//开始迷宫得触发器
public class StartMiGongTrigger : MonoBehaviour
{
    public GameMgr gamemgr;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gamemgr.m_IsStart = true;
        }
    }
}
