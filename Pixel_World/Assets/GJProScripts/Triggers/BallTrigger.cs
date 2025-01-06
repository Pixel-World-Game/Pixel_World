using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    public TipPanel tip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tip.SetTip("The orb of energy has been collected");
            PlayerData.GetInstance().m_NengLiangBallNums++;
            if (PlayerData.GetInstance().m_NengLiangBallNums >= 7)
                PlayerData.GetInstance().m_NengLiangBallNums = 7; 

            GameObject.Destroy(gameObject);
        }
    }
}
