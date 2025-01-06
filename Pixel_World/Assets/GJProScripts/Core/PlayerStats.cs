using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
public class PlayerStats : MonoBehaviour
{
    public GameObject HpBar;


    //�Ƿ����
    public bool m_IsDrop;

    //��ǰѪ��
    public int m_CurHp;

    //���Ѫ��
    public int m_MaxHp; 
    
    //HipUI
    public GameObject HitEffectUI;

    //��Ϸ����
    public GameObject GameOver;

    //�˺�����
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

    //����Ѫ��Ч��ʧ
    public void HitEffectHide()
    {
        HitEffectUI.SetActive(false);
    }
}
