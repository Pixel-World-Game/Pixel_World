using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RoleBulletController : MonoBehaviour {

 
    private int bullets = 50;
    public int HitNumber;
    public Rigidbody bullet;
    private GameObject firePoint;
    public Texture2D texture;
    public int blood = 100;
    public int BloodHp = 3;
   private bool isShowBlood = true;
    public Texture2D bloodBgTexture;  
    public Texture2D bloodTexture;
    public Texture2D AllRed;
    private float howAlpha;
 
    public GameObject Gun;

    public GameObject[] MyWinEnd;
    public static int Level;
    public int ThisHit;
    public Text[] texts;
    public AudioSource[] audioSources;
    bool i;
    public GameObject MonsterObj;
    void Start () {
        firePoint = GameObject.Find("firePoint");
        blood = 100;
        Cursor.lockState = CursorLockMode.Locked;
        BloodHp = 3;
        HitNumber = 20;
        ThisHit = 20;
        Time.timeScale = 1;
        // audioSources[0].volume = Begin.YinYue;
        // audioSources[1].volume = Begin.YinYue;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
           
          
        }
        if (Level >= 100)
        {
            Level = 0;
            ThisHit += 10;
        }
       // texts[0].text = bullets.ToString();
       // texts[1].text = Level.ToString();
        // text.text = "bullets:" + bullets.ToString();
        // text1.text = "score:" + score1.ToString();
        //text2.text = BloodHp.ToString();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) /*&& bullets > 0*/&&i==false)
            {
               // bullets--;              
            Vector3 target = ray.GetPoint(20);
            Rigidbody clone = (Rigidbody)Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            clone.velocity = (target - firePoint.transform.position) * 3;         
            Gun.SendMessage("shootAudio");
            if (Physics.Raycast(ray, out hit, 50))
            {
          
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Monster")
                {
                    hit.collider.GetComponent<Monster>().HP -= ThisHit;
                    if (hit.collider.GetComponent<Monster>().HP <= 0)
                    {
                        Level += 20;
                        Destroy(hit.collider.gameObject);
                    }
                }

            }
        }
 
        firePoint.transform.LookAt(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(20));

        if (blood <= 0)
        {
            Time.timeScale = 0;
            MyWinEnd[0].SetActive(true);
          //  Debug.Log("Game Over!");
          
        }
        if (MonsterObj.transform.childCount == 0)
        {
            //  Time.timeScale = 0;
            i = true;
            Cursor.lockState = CursorLockMode.None;
            MyWinEnd[2].SetActive(true);
            //  MyWinEnd[1].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       /* if (other.tag == "1")
        {
            addBlood();
            Destroy(other.gameObject);
        }
        if (other.tag == "2")
        {
            addBullet();
            Destroy(other.gameObject);
        }*/
        if (other.tag == "Zd")
        {
            MyWinEnd[1].SetActive(true);
            // addBullet();
            // Destroy(other.gameObject);
        }
        
    }
  /*  public void addBlood()
    {             
            blood += 50;
        if (blood > 100)
        {
            blood = 100;
        }
    }
    public void addBullet()
    {
        
        bullets += 20;
       
    }*/

    public void ZaiLai()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void TuiChu()
    {
        Application.Quit();
    }
       
   
    void OnGUI()
    {
  
        //GUI.Label(new Rect(10, Screen.height - 30, 150, 50), "子弹个数 x" + bullets + "   分数"+ score);
   
        Rect rect = new Rect(Input.mousePosition.x - (texture.width / 2),
        Screen.height - Input.mousePosition.y - (texture.height / 2),
        texture.width, texture.height);
        GUI.DrawTexture(rect, texture);
      
       
        GUI.DrawTexture(new Rect(130, 50, bloodBgTexture.width*2, bloodBgTexture.height*2), bloodBgTexture);
        GUI.DrawTexture(new Rect(130, 50, bloodTexture.width * (blood * 0.01f) * 2, bloodTexture.height * 2), bloodTexture);
    
        Color alpha = GUI.color;
        howAlpha = (100.0f - blood) / 120.0f;
        if(howAlpha < 0.42)
        {
            howAlpha = 0;
        }
        alpha.a = howAlpha;
        GUI.color = alpha;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), AllRed);
    }
}
