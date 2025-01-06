using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ò©Æ¿´¥·¢Æ÷
public class HpItemTrigger : MonoBehaviour
{
    public int Hp;
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStats>().m_CurHp += Hp;
            if (other.gameObject.GetComponent<PlayerStats>().m_CurHp >= other.gameObject.GetComponent<PlayerStats>().m_MaxHp)
            {
                other.gameObject.GetComponent<PlayerStats>().m_CurHp = other.gameObject.GetComponent<PlayerStats>().m_MaxHp;
            }
                
            GameObject.Destroy(gameObject);
        }
    }
}
