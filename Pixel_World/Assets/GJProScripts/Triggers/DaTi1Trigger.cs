using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaTi1Trigger : MonoBehaviour
{
    public GameObject DaTiui;

    private void OnMouseDown()
    {
        DaTiui.SetActive(true);    
    }
}
