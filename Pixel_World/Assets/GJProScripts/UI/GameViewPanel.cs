using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameViewPanel : MonoBehaviour
{
    //血条
    public Image m_HpBar;

    //剩余时间
    public Text m_LeftTimeT;

    public GameMgr GameMgr;

    public Text m_CurNengLiangBallNums;

    public Text m_MaxNengLianBallNums;

    public Text m_CurKillNumsT;
    public Text m_MaxKillNumsT;

    // Update is called once per frame
    void Update()
    {
        PlayerStats states = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        //血条更新
        if(m_HpBar!=null)
        {
            m_HpBar.fillAmount = (float)states.m_CurHp / states.m_MaxHp;
        }

        //剩余时间更新
        if(m_LeftTimeT!=null)
        {
            m_LeftTimeT.text = ((int)GameMgr.m_LeftTime).ToString();
        }

        //能量球
        if(m_CurNengLiangBallNums!=null) 
            m_CurNengLiangBallNums.text = PlayerData.GetInstance().m_NengLiangBallNums.ToString();
        if(m_MaxNengLianBallNums!=null) 
            m_MaxNengLianBallNums.text = PlayerData.GetInstance().m_MaxNengLianBalNums.ToString();

        if(m_CurKillNumsT!=null)
        {
            m_CurKillNumsT.text = PlayerData.GetInstance().m_CurKillEnmeyNums.ToString();
        }

        if (m_MaxKillNumsT != null)
        {
            m_MaxKillNumsT.text = PlayerData.GetInstance().m_MaKillEnemyNums.ToString();
        }

    }
}
