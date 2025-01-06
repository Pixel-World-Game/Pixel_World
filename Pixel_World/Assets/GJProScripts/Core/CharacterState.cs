using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//角色属性
public class CharacterState : MonoBehaviour
{  
    

    //血量
    public int m_Hp; 
    public int m_MaxHp; 
    
    public int m_Damge = 5; 
   

    //碰撞体半径
    public int m_DistanceToPlayer; 
    
    public void TakeDamge(GameObject _atter)
    { 
        m_Hp -= _atter.GetComponent<CharacterState>().m_Damge; 
       
        if (m_Hp<=0)
        {
            if(tag == "E")
            {
              //  GameObject.Find("PlayerData").GetComponent<PlayerData>().IsGameWin();
            }
            else
            {
             //   GameObject.Find("PlayerData").GetComponent<PlayerData>().IsGamOver();
            } 

            GameObject.Destroy(gameObject); 
        }  
    }  

    private void Update()
    {
        if (tag == "Player")
        {
            if (m_Hp <= 0)
            {
                //游戏失败

            }
        }
    }
}
