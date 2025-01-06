using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BaseManager<PlayerData>
{
    //是否已经拿到钥匙
    public bool m_IsGetKey;

    //是否和木屋外NPC对话
    public bool m_IsNpcDuiHua;

    //是否已经打开
    public bool m_IsOpen;

    //当前碎片个数
    public int m_CurSuiPianNums;

    //是否拾取宝箱钥匙
    public bool m_IsGetBaoxiangkey;

    //是否拾取笼子钥匙
    public bool m_IsGetLongZiKey;

    //收集能量球个数
    public int m_NengLiangBallNums;
    public int m_MaxNengLianBalNums = 7;

    //杀敌数量
    public int m_CurKillEnmeyNums = 14;
    public int m_MaKillEnemyNums = 14;


    //增加碎片
    public void AddSuiPian()
    {
        m_CurSuiPianNums++;
        if(m_CurSuiPianNums>=9)
        {
            Debug.Log("拾取完毕,游戏胜利");
            GameObject.Find("Canvas").GetComponent<UIMgr>().TipUI.GetComponent<TipPanel>().SetTip(" 地图碎片全部找到，请拼接起来得到工厂地图");
            GameObject.Find("Canvas").GetComponent<UIMgr>().GoToPinTuUI(); 
        }
    } 

    //能量球是否收集满
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
