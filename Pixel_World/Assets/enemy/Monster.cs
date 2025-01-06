using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Monster : MonoBehaviour {
    public int thisone;
    public int ThisState;
    public GameObject Player;
    public float AttackTime;
    public GameObject[] Bullet;
    public float[] AllDis;
    public float HP;
    public Slider slider1;
    public int Thisone;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        slider1.maxValue = HP;
    }
	
	// Update is called once per frame
	void Update () {
        slider1.value = HP ;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
           // Player.GetComponent<Main>().MyScore += 1;
        }
        if (thisone == 0)
        {
            if (ThisState == 0)
            {
                this.GetComponent<NavMeshAgent>().enabled = false;
            }
            else if (ThisState == 1)
            {
                this.GetComponent<NavMeshAgent>().enabled = true;
                this.GetComponent<NavMeshAgent>().SetDestination(Player.transform.position);
            }
            else if (ThisState == 2)
            {
                if(Thisone==0)
                this.GetComponent<Animator>().SetInteger("State", 0);
                else
                    this.GetComponent<Animation>().Play("idle");
                this.GetComponent<NavMeshAgent>().enabled = false;
                AttackTime += Time.deltaTime;
                if (AttackTime >= 1.5f)
                {
                    ThisState = 3;
                    if (Random.Range(0, 2) == 1)
                    {
                        this.transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));
                    }
                    AttackTime = 0;
                    if (Thisone == 0)
                        this.GetComponent<Animator>().SetInteger("State", 1);
                    else
                        this.GetComponent<Animation>().Play("shoot");
                    Bullet[0].SetActive(true);
                    Invoke("EndAttack", 1);
                }
            }
            else
            {

            }
            float Dis = Vector3.Distance(this.transform.position, Player.transform.position);
            if (Dis < AllDis[0] && Dis > AllDis[1])
            {
                ThisState = 1;
                if (Thisone == 0)
                    this.GetComponent<Animator>().SetInteger("State", 2);
                else
                    this.GetComponent<Animation>().Play("walk");
            }
            else if (Dis <= AllDis[1])
            {
                if(ThisState != 3)
                ThisState = 2;
               
            }
            else
            {
                ThisState = 0;
                if (Thisone == 0)
                    this.GetComponent<Animator>().SetInteger("State", 0);
                else
                    this.GetComponent<Animation>().Play("idle");
            }
        }
        if (thisone == 1)
        {
            if (ThisState == 0)
            {
                this.GetComponent<NavMeshAgent>().enabled = false;
            }
            else if (ThisState == 1)
            {
                this.GetComponent<NavMeshAgent>().enabled = true;
                this.GetComponent<NavMeshAgent>().SetDestination(Player.transform.position);
            }
            else if (ThisState == 2)
            {
                this.transform.GetChild(2).GetComponent<Animation>().Play("shootfp");
                this.GetComponent<NavMeshAgent>().enabled = false;
                AttackTime += Time.deltaTime;
                if (AttackTime >= 1.5f)
                {

                    this.transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));

                    AttackTime = 0;

                    GameObject gameObject1 = GameObject.Instantiate(Bullet[0], Bullet[1].transform.position, Quaternion.identity);
                    Vector3 dis1 = new Vector3(Player.transform.position.x, Bullet[1].transform.position.y, Player.transform.position.z) - gameObject1.transform.position;
                    gameObject1.transform.forward = dis1;
                }
            }
            else
            {

            }
            float Dis = Vector3.Distance(this.transform.position, Player.transform.position);
            if (Dis < AllDis[0] && Dis > AllDis[1])
            {
                ThisState = 1;
                this.transform.GetChild(2).GetComponent<Animation>().Play("walk");
            }
            else if (Dis <= AllDis[1])
            {
                if (ThisState != 3)
                    ThisState = 2;

            }
            else
            {
                ThisState = 0;
                this.transform.GetChild(2).GetComponent<Animation>().Play("shootfp");
            }
           
          
        }
	}
    public void EndAttack()
    {
        ThisState = 0;
        if (Thisone == 0)
            this.GetComponent<Animator>().SetInteger("State", 0);
        else
            this.GetComponent<Animation>().Play("idle");
    }
}
