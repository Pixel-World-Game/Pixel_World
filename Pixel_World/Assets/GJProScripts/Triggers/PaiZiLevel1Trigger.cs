using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ӵ�����
public class PaiZiLevel1Trigger : MonoBehaviour
{
    public GameObject UI;

    private void OnMouseDown()
    {
        UI.SetActive(true);    
    }
}
