using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Hit1 : MonoBehaviour {
    public int Thisone;
    float ThisTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ThisTime += Time.deltaTime;
        if (ThisTime >= 1)
        {
            ThisTime = 0;
            this.gameObject.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                this.gameObject.SetActive(false);
                other.GetComponent<Player>().HP -= Thisone;
            }
            else
            {
                this.gameObject.SetActive(false);
                other.GetComponent<RoleBulletController>().blood -= Thisone;
            }

        }
    }
}
