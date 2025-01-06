using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoXiangGai1Trigger : MonoBehaviour
{
    public Animator A;

    public TipPanel tip;
    private void OnMouseDown()
    {
         if(PlayerData.GetInstance().m_IsGetBaoxiangkey)
        {
            A.enabled = true;
        }
         else
        {
            tip.SetTip("需要钥匙才能打开");
        }
    }
}
