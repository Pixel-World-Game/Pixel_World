using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulleteTrigger : MonoBehaviour
{
    public GameObject HitEffect;

    public string m_Tag  ;

    public float m_Speed;

    public int m_Damge;   
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
    } 

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == m_Tag)
        {
            //»÷´òÌØÐ§
            GameObject.Instantiate(HitEffect, transform.position, Quaternion.identity);
            other.GetComponent<PlayerStats>().TakeDamge(m_Damge);
            GameObject.Destroy(gameObject);
        }
    }
}
