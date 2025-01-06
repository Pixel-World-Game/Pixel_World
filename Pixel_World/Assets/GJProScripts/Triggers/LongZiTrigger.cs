using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongZiTrigger : MonoBehaviour
{
    //���ſ϶���
    public Animator OpenDoorA;

    public GameObject Tip;

    private bool isPress;

    public TipPanel tipui;
    
    public  GameObject GameWin;
         
    void Update()
    {
        if(isPress)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //�ж��Ƿ�õ�Կ��
                if(PlayerData.GetInstance().m_IsGetLongZiKey)
                {
                    OpenDoorA.enabled = true;
                    tipui.SetTip("The cage is open.!");
                    Tip.SetActive(false);
                    Invoke("GameWin1", 1.5f);
                }
                else
                {
                    tipui.SetTip("Need a key!");
                }  
            }
        }
    }

    public void GameWin1()
    {
        GameWin.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Tip.SetActive(true);
            isPress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.tag == "Player")
            {
                isPress = false;
                Tip.SetActive(false);
            }
        }
    }
}
