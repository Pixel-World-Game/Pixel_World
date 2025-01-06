using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//npc
public class Npc : MonoBehaviour
{   
    //对话数据
    public string[] m_Datas = { "",
                                ""
                                };

    //对话面板
    public DuiHuaPanel m_DuiHuaPanel;

    //是否开启对话
    public bool m_IsStartDuiHua;

    //对话索引
    private int m_DuiHuaIndex;

    //寻路导航
    private NavMeshAgent m_Agent; 

    //路径点
    public Transform[] m_Paths;

    private float m_CurTime;

    public GameObject DaTiUi;

    private void Start() {
       
        m_DuiHuaIndex = 0;
        m_Agent = GetComponent<NavMeshAgent>();
    } 

    private void Update() {

        
        
        if(m_IsStartDuiHua)
        {
            //按F 对话继续
            if(Input.GetKeyDown(KeyCode.F))
            {
                m_DuiHuaIndex++;
                if(m_DuiHuaIndex>=m_Datas.Length)
                {
                    //关闭面板
                    m_DuiHuaPanel.gameObject.SetActive(false);
                    m_IsStartDuiHua =false;
                    m_DuiHuaIndex = 0;
                    
                    if(SceneManager.GetActiveScene().name == "Game2" )
                    {
                        PlayerData.GetInstance().m_IsNpcDuiHua = true;
                    }


                    if (SceneManager.GetActiveScene().name == "Game2" && name == "DaTiNPC")
                    {
                        DaTiUi.SetActive(true);
                    }

                    return;
                }
                m_DuiHuaPanel.SetStr(m_Datas[m_DuiHuaIndex]);
            }
        } 
    } 

    public bool IsRange(float _range)
    {
        float dis = Vector3.Distance(GameObject.FindWithTag("Player").transform.position,transform.position);
        if(dis <= _range)
        {
            return true;
        }

        return false;
    }

    private void OnMouseOver() {

        //这里来处理UI出现时候鼠标进入非锁定
        if (EventSystem.current != null)
        {
            //判断鼠标是不是在UI上面
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }

        if (IsRange(5))
        {
            if(Input.GetMouseButtonDown(0))
            {   
                 
                //激活对话面板.让NPC与玩家对话
                m_IsStartDuiHua = true;
                m_DuiHuaPanel.SetStr(m_Datas[m_DuiHuaIndex]);
                //让角色朝向玩家
                Vector3 dir = GameObject.FindWithTag("Player").transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir.normalized); 
            }
        }
    }
     
}
