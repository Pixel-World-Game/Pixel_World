using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Move : MonoBehaviour {

    public GameObject Player;
	// Use this for initialization
	void Start () {
      

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(this.transform.forward * Time.deltaTime*3,Space.World);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.tag == "Monster")
            {
                other.GetComponent<Monster>().HP -= 20;
            }
            Destroy(this.gameObject);
        }
    }
}
