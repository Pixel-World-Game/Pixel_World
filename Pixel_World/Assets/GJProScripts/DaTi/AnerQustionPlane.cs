using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[Serializable]
//题目结构
public class QuestionTextData
{    
    //内容
    public string Connnet;

    //选项
   public string[] ChooseConnnet;

    //答案
   public int[] anwerindex;
}

public class AnerQustionPlane : MonoBehaviour
{
     //错误提示
    public GameObject m_ErrorTip;
    
    //题目数据-根据这些数据来对答题界面进行初始化
    public QuestionTextData[] data;

    //答题索引
    [HideInInspector]
    public int index; 
    
    //返回上级菜单按钮
    public Button m_ExtBtn;

    //下一题目按钮
    public Button m_Ok;

    //题目内容
    public Text m_Connet;

    //选择项目
    public Text[] m_ChooseConnet;

    //打勾组件数组
    public Toggle[] m_Toggles;

    public GameObject TT;

    private void OnEnable() {
        m_ExtBtn.gameObject.SetActive(true);
        m_Ok.gameObject.SetActive(true);
        for(int i=0;i<m_Toggles.Length;i++)
        {
            m_Toggles[i].gameObject.SetActive(true);
        }
        InitData(); 
    }

    private void Start() { 
        m_ExtBtn.onClick.AddListener(()=>{
            gameObject.SetActive(false);
        });

        //提交答案
        m_Ok.onClick.AddListener(()=>{ 

            if (IsAnwerOk())
            {   
                //进入下一个题目
                index++;
                if(index>=data.Length)
                {
                    TT.SetActive(true);
                    m_Connet.text = "";
                    for (int i = 0; i <m_ChooseConnet.Length; i++)
                    {
                        m_ChooseConnet[i].text = "";
                    }

                    for(int i=0;i<m_Toggles.Length;i++)
                    {
                        m_Toggles[i].gameObject.SetActive(false);
                    }

                    m_ExtBtn.gameObject.SetActive(false);
                    m_Ok.gameObject.SetActive(false);

                    PlayerData.GetInstance().AddSuiPian();
                    Invoke("Hide", 3.0f);
                }
                else
                {   
                    //设置下一个题目内容
                    SetQesqionsText(data[index].Connnet,data[index].ChooseConnnet); 
                } 
            }
            else
            {
                //提示选择错误
                m_ErrorTip.SetActive(true);
                //延迟
                Invoke("HideErrorTip",1.5f);
                Invoke("E", 1.6f);
            }
            
        });

        InitData(); 
    }

    //隐藏面板
    public void Hide()
    {
       gameObject.SetActive(false);
    }

    //隐藏提示
    public void HideErrorTip()
    {
        m_ErrorTip.SetActive(false);
    }

    public void E()
    {
        gameObject.SetActive(false);
    }

    //判断是否答对
    public bool IsAnwerOk()
    {
        //记录被打勾的索引
        int[] recodindex = new int[data[index].anwerindex.Length];
        int index1 = 0;
        for(int i=0;i<m_Toggles.Length;i++)
        {
            if(m_Toggles[i].isOn == true)
            { 
                 recodindex[index1] = i;
                 index1++;
            }
        }

        for(int j=0;j<recodindex.Length;j++)
        {
            if(recodindex[j] != data[index].anwerindex[j])
                return false;
        }

        return true; 
    }

    public void InitData()
    {
        index = 0;
        SetQesqionsText(data[index].Connnet,data[index].ChooseConnnet);
    }

    //设置题目内容
    public void SetQesqionsText(string _connet,string[] choosec)
    {
        m_Connet.text = _connet;
        for(int i=0;i<m_ChooseConnet.Length;i++)
        {
            m_ChooseConnet[i].text = choosec[i];
        }
    }
}
