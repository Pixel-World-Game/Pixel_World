using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ʼ�Թ��ô�����
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
