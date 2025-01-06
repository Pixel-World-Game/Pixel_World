using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Éä»÷
public class Shoot : MonoBehaviour
{
    public AudioClip m_ShootClip;

    public Transform pos;

    public GameObject REs;

    public GameObject Target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Instantiate(REs, pos.position, pos.rotation);

            GetComponent<AudioSource>().PlayOneShot(m_ShootClip);
        }   
    }

    public void ShootEnemy() 
    {
        GameObject.Instantiate(REs, pos.position, pos.rotation);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Target = null;
        }
    }
}
