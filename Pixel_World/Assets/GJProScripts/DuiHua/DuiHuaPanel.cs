using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//对话面板
public class DuiHuaPanel : MonoBehaviour
{    
    //文本
    public Text m_Str; 

    //设置文本
    public void SetStr(string _str)
    {
        m_Str.text = _str;
        gameObject.SetActive(true);
    }
 
}
