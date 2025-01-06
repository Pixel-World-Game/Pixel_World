using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    void OnCollisionEnter(Collision collisionInfo)
    {
        Destroy(this.gameObject);
    }
    public float EndTime;
    // Use this for initialization
    void Start () {
        
        //float time = 0.4f;
       
        Destroy(this.gameObject, EndTime);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
