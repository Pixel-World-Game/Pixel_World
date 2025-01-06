using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//石头出发起
public class ShiTouTrigger : MonoBehaviour
{
    public TipPanel tipui;

    public Animator A;

    public bool m_IsTui;

     
    void Update()
    {
        if(m_IsTui)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                A.enabled = true;
            } 
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //判断珠子收集了没
            if(PlayerData.GetInstance().IsGetAllNenegLiangBall())
            {
                tipui.SetTip("Press E to push, you can push the stone door open”");
                m_IsTui = true;
            }
            else
            {
                tipui.SetTip("This boulder is too large, and not enough energy balls have been collected so far to push it away");
            } 
            
        }
            
    }
}
