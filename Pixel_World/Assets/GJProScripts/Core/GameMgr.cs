using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ϸ������
public class GameMgr : MonoBehaviour
{
    //ʣ��ʱ��
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
                Debug.Log("��Ϸ����");
                GameOverUI.SetActive(true);
            }
        } 
    }
}
