using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Կ�״�����
public class BaoXiangKeyTrigger : MonoBehaviour
{
    private void OnMouseDown()
    {
        PlayerData.GetInstance().m_IsGetBaoxiangkey = true;
        GameObject.Destroy(gameObject);
    }
}
