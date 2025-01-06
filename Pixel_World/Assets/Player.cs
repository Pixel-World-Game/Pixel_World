using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour {

    public GameObject[] gameObjects;
    public float GameTime;

    public int HP;
    public Text HPtext;
    public GameObject Bullet;
    public int BulletNum,ptint;
    float ShootTime;
    public GameObject[] UI;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        BulletNum = 20;

    }
	
	// Update is called once per frame
	void Update () {
      
        gameObjects[0].GetComponent<Text>().text = ((int)GameTime).ToString();
        if (HP >= 0)
        {
            HPtext.text = "HP:" + HP.ToString();
        }
        if (ptint >= 9)
        {
            gameObjects[5].SetActive(true);
            gameObjects[3].SetActive(true);
        }
        GameTime -= Time.deltaTime;
        if (GameTime <= 0)
        {
            Time.timeScale = 0;
            gameObjects[1].SetActive(true);
        }
        if (HP <= 0)
        {
            Time.timeScale = 0;
            gameObjects[1].SetActive(true);
        }
        ShootTime -= Time.deltaTime;
        if (Input.GetMouseButton(0)&& ShootTime<=0)
        {
            if (BulletNum > 0)
            {
                ShootTime = 0.5f;
                BulletNum -= 1;
                GameObject gameObject1 = GameObject.Instantiate(Bullet, this.transform.position, Quaternion.identity);
                gameObject1.transform.forward = this.transform.forward;
            }
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "1")
        {
            Time.timeScale = 0;
            gameObjects[2].SetActive(true);
        }
        if (other.tag == "2")
        {
            if (Random.Range(0, 10) >= 3)
                GameTime += 10;
            else
                HP += 10;
            if (HP >= 100)
            {
                HP = 100;
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "3")
        {
            HP -= 10;
            Instantiate(gameObjects[4], transform.position, transform.rotation);
           // Destroy(other.gameObject);
        }
        if (other.tag == "4")
        {
            ptint += 1;
           // Instantiate(gameObjects[4], other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
        if (other.tag == "Men")
        {
            //  Time.timeScale = 0;
            //  gameObjects[2].SetActive(true);
            UI[1].SetActive(true);
        }
        if (other.tag == "Key")
        {
            //  Time.timeScale = 0;
            //  gameObjects[2].SetActive(true);
            Destroy(other.gameObject);
            UI[0].SetActive(true);
           // gameObjects[3].SetActive(true);
        }
        if (other.tag == "Npc")
        {
            //  Time.timeScale = 0;
            //  gameObjects[2].SetActive(true);
            UI[2].SetActive(true);
            gameObjects[6].SetActive(true);
        }
        if (other.tag == "Next")
        {
            SceneManager.LoadScene(2);
        }
       

    }
    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }
    public void First()
    {
        SceneManager.LoadScene(0);
    }
}
