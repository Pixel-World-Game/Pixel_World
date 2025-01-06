using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//属性
public class PlayerStats : MonoBehaviour
{
    public GameObject HpBar;


    //是否掉落
    public bool m_IsDrop;

    //当前血量
    public int m_CurHp;

    //最大血量
    public int m_MaxHp; 
    
    //HipUI
    public GameObject HitEffectUI;

    //游戏结束
    public GameObject GameOver;

    //伤害计算
    public void TakeDamge(int _value)
    {
        if(tag == "Player")
        {
            HitEffectUI.SetActive(true);
            Invoke("HitEffectHide", 1);
        }
        m_CurHp -= _value;
        if(m_CurHp<=0)
        {
            if(tag == "Player")
            {
                GameOver.SetActive(true);
                GetComponent<Animator>().SetBool("Die", true);
            }

            if(tag == "Enemy")
            {
                GetComponent<Animator>().ResetTrigger("Attack");
                GetComponent<Animator>().SetBool("Die", true);
                GetComponent<Collider>().enabled = false;
                PlayerData.GetInstance().KillEnemeyNum();
                if (m_IsDrop)
                    GetComponent<DropItem>().DropTrop();
                GameObject.Destroy(gameObject, 3);

                if (HpBar != null)
                    HpBar.SetActive(false);
            }

            m_CurHp = 0;
        }
    }

    //被扣血特效消失
    public void HitEffectHide()
    {
        HitEffectUI.SetActive(false);
    }
}
