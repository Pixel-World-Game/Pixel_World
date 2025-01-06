using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//带刺陷阱触发器
public class XianJingCiTrigger : MonoBehaviour
{
    //是否进入
    private bool m_IsGoIn;

    private float m_CurTime;

    public GameObject m_Target;

    public TipPanel tip;

    public int m_Damge;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_IsGoIn = true;
            m_Target = other.gameObject;
            tip.SetTip("Watch out! There is a trap");
            if (m_Target != null)
            {
                m_Target.GetComponent<PlayerStats>().TakeDamge(m_Damge);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_IsGoIn = false;
            m_Target = null;
        }
    }

    private void Update()
    {
        if(m_IsGoIn)
        {
            m_CurTime += Time.deltaTime;
            if(m_CurTime>=1.5f)
            {
                if(m_Target!=null)
                {
                    m_Target.GetComponent<PlayerStats>().TakeDamge(2);
                }
                m_CurTime = 0;
            }
        }
    }
}
