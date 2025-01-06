using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class TipPanel : MonoBehaviour
{
    public Text m_TipText;

    public UnityAction func;
    public void SetTip(string _str,UnityAction fun = null)
    {
        func = fun;
        m_TipText.text = _str;
        gameObject.SetActive(true);
        Invoke("Hide", 1);
    }

    public void Hide()
    {
        if(func!=null)
        {
            func.Invoke();
        }
        gameObject.SetActive(false);
    }
}
