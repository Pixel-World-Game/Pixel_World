using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BaseManager<PlayerData>
{
    //�Ƿ��Ѿ��õ�Կ��
    public bool m_IsGetKey;

    //�Ƿ��ľ����NPC�Ի�
    public bool m_IsNpcDuiHua;

    //�Ƿ��Ѿ���
    public bool m_IsOpen;

    //��ǰ��Ƭ����
    public int m_CurSuiPianNums;

    //�Ƿ�ʰȡ����Կ��
    public bool m_IsGetBaoxiangkey;

    //�Ƿ�ʰȡ����Կ��
    public bool m_IsGetLongZiKey;

    //�ռ����������
    public int m_NengLiangBallNums;
    public int m_MaxNengLianBalNums = 7;

    //ɱ������
    public int m_CurKillEnmeyNums = 14;
    public int m_MaKillEnemyNums = 14;


    //������Ƭ
    public void AddSuiPian()
    {
        m_CurSuiPianNums++;
        if(m_CurSuiPianNums>=9)
        {
            Debug.Log("ʰȡ���,��Ϸʤ��");
            GameObject.Find("Canvas").GetComponent<UIMgr>().TipUI.GetComponent<TipPanel>().SetTip(" ��ͼ��Ƭȫ���ҵ�����ƴ�������õ�������ͼ");
            GameObject.Find("Canvas").GetComponent<UIMgr>().GoToPinTuUI(); 
        }
    } 

    //�������Ƿ��ռ���
    public bool IsGetAllNenegLiangBall()
    {
        if(m_NengLiangBallNums>= m_MaxNengLianBalNums)
        {
            return true;
        }

        return false; 
    }

    public void KillEnemeyNum()
    {
        m_CurKillEnmeyNums--;
        if(m_CurKillEnmeyNums<=0)
        {
            m_CurKillEnmeyNums = 0;
        }
    }

}
