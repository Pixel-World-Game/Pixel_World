using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CodePanel : MonoBehaviour
{
    public string m_OrcCode = "213151";

    public InputField m_InPUT;

    public Button okbtn;

    public TipPanel tip;

    public BaoXiangGai2Trigger trr;

    void Start()
    {
        okbtn.onClick.AddListener(() =>
        {
            if(m_InPUT.text == m_OrcCode)
            {
                trr.A.enabled = true;
                gameObject.SetActive(false);
            }
            else
            {
                tip.SetTip("密码错误，无法打开");
            } 
        });
    } 
}
