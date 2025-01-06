using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//±¦Ïä¸Ç2´¥·¢Æ÷
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
