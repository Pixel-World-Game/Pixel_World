using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public GameObject TipUI;

    public GameObject GamWinUI;

    public GameObject PinTuUI;

    //����ƴͼUI
    public void GoToPinTuUI()
    {
        Invoke("PinTu", 3);
    }

    public void PinTu()
    {
        PinTuUI.SetActive(true);
    }
}
