using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SuiPian : MonoBehaviour
{
    public Text CurSuiNumsT;

    public Text MaxSuiNumsT; 

    // Update is called once per frame
    void Update()
    {
        CurSuiNumsT.text = PlayerData.GetInstance().m_CurSuiPianNums.ToString();
        MaxSuiNumsT.text = "9";
    }
}
