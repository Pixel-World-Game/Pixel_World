using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
//ÀÈ∆¨
public class ItemSuiPian : MonoBehaviour
{
    public GameObject ShiquTip; 

    private void OnMouseDown()
    {
        ShiquTip.SetActive(false);
        Debug.Log("Pick Up");
        PlayerData.GetInstance().AddSuiPian();
        GameObject.Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        ShiquTip.SetActive(true);
        GetComponent<Highlighter>().ConstantOn();
    }

    private void OnMouseExit()
    {
        ShiquTip.SetActive(false);
        GetComponent<Highlighter>().ConstantOff();
    }
}
