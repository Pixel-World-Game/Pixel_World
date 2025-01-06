using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//±¦ÏäÔ¿³×´¥·¢Æ÷
public class BaoXiangKeyTrigger : MonoBehaviour
{
    private void OnMouseDown()
    {
        PlayerData.GetInstance().m_IsGetBaoxiangkey = true;
        GameObject.Destroy(gameObject);
    }
}
