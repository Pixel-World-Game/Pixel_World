using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����2������
public class BaoXiangGai2Trigger : MonoBehaviour
{
    public Animator A;

    public GameObject UI;

    private void OnMouseDown()
    {
        UI.SetActive(true);
        //A.enabled = true;
    }

}
