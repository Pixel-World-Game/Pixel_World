using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    //������Ʒ
    public GameObject m_DropItem;

    public void DropTrop()
    {
        GameObject.Instantiate(m_DropItem, transform.position, transform.rotation);
    }
}
