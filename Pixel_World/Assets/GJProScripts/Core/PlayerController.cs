using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
//这个角色控制器 跳跃 用的是刚体+碰撞来实现的
public class PlayerController : MonoBehaviour
{ 
    public float speed = 10.0f;

    public float jumpForce;

    public bool IsLockMouse;

    private Animator m_Animator;   

    private Transform cam;

    float turnSmoothVelocity;

    private float horizontal;

    private float vertical;

    private float turnSmoothTime = 0.1f;

    private bool canjump = true;

    private Rigidbody rb;    

    void Start()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>(); 
       
           // Cursor.lockState = CursorLockMode.Locked;
        
    }  
   
    void Update()
    { 
        Jump();

        //if (Input.GetMouseButton(1))
        //{
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") *Time.deltaTime * 120, 0));  
        //}
    }    
    
    //跳跃
    public void Jump()
    { 
        
        if(Input.GetKeyDown(KeyCode.Space) && canjump)
        {    
            //为刚体添加一个向上的力
           // rb.AddForce(Vector3.up * (jumpForce));
           // canjump = false; 
        } 

       //当刚体速度小于-1的时候就证明下落了
       if(rb.velocity.y<=-1)
       { 
            //这个时候给它加一个向下的力量
            rb.AddForce(-Vector3.up);
       }
    }  
     

    private void FixedUpdate() 
    { 
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
            
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            //float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

           // float targetAngle =   cam.eulerAngles.y;

           //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

           // transform.rotation = Quaternion.Euler(0f, angle, 0f);

           // Vector3 moveDir = new Vector3(horizontal  , 0, vertical); //  Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

           // rb.velocity = rb.velocity.y * Vector3.up + moveDir * speed ;

            transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * speed);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if(canjump)
        {
            if (horizontal != 0 || vertical != 0)
            {
                //播放行走
               m_Animator.SetBool("Run", true);
            }
            else
            {
               m_Animator.SetBool("Run", false);
            }
        }  
    }

    private void LateUpdate() 
    {
        transform.position = rb.transform.position; 
    }

    private void OnCollisionEnter(Collision collision) {
        
        //if(collision.gameObject.CompareTag("Ground"))
        //{
        //    canjump = true;
        //    //m_Animator.SetBool("Jump", false);
        //}
    }

    public Shoot shoot;
    public void Event_Shoot()
    {
        shoot.ShootEnemy();
    }
   
}
