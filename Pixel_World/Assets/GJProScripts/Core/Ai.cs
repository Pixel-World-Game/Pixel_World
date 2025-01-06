using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//状态
public enum E_State
{
    Idle,   //待机

    Run,    //奔跑
    
    Attack, //攻击

    GoBack  //撤退
} 

//AI行为控制脚本
public class Ai : MonoBehaviour
{
    public GameObject BulletRes;

    public Transform m_FirePos;

    //寻路导航组件
    public NavMeshAgent m_Anget;

    //目标
    public GameObject m_Target; 
    
    //动画组件
    public Animator m_Animator; 
    
    //当前的目标点
    private Vector3 m_CurPosition; 
    
    //攻击频率的计时器
    private float time; 
    
    //当前状态
    public E_State m_state; 

    //视野范围
    public float m_Sight;

    //最大行动范围
    public float m_MaxActionRange;

    //巡逻范围
    public float m_PatrolRange; 
    
    //攻击范围
    public float m_AttackRange = 3; 
     
    //初始位置 
    private Vector3 m_OrgPos; 

    //是否巡逻
    public bool m_IsPatrol;

    public GameObject test; 
    void Start()
    { 
        m_Anget = GetComponent<NavMeshAgent>();
        m_state = E_State.Idle;
        m_Target =GameObject.FindGameObjectWithTag("Player"); 
        m_OrgPos = transform.position;
        m_CurPosition = GetRandomPoint();
        m_Anget.SetDestination(m_CurPosition);
    }
    
    //获取随机的一个点，在一个范围内
    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(-m_PatrolRange,m_PatrolRange);
        
        float y = Random.Range(-m_PatrolRange,m_PatrolRange);

        Vector3 pos = new Vector3(m_OrgPos .x + x,m_OrgPos.y,m_OrgPos.z +y);
        
       pos = NavMesh.SamplePosition(pos,out NavMeshHit hit,m_PatrolRange,1)?pos:transform.position; 
        return pos;
    }

    //是否到达目的点
    public bool IsReacthPoint()
    {
        float dis = Vector3.Distance(transform.position,m_CurPosition);
     //   Debug.Log("dis:" + dis);
        if(dis <= 1.1f)
            return true;
        return false;
    }

    void Update()
    {    
        //当HP为0 不做后面的逻辑
        if (GetComponent<PlayerStats>().m_CurHp <= 0)
            return;

        switch(m_state)
        {
            case E_State.Idle:          //在待机状态会做什么
                //如果巡逻激活
                if(m_IsPatrol)
                {
                    //是否到达目的地
                    if(IsReacthPoint())
                    { 
                        m_CurPosition = GetRandomPoint(); 
                    }
                    else
                    { 
                        m_Animator.SetBool("Run", true);
                        m_Anget.SetDestination(m_CurPosition); 
                    } 
                }
                else
                { 
                    m_Animator.SetBool("Run", false);
                }
               
                //目标是否在视野内
                if (IsSight(m_Target))      //待机状态下 切换到追击状态的条件，满足就切换
                {     
                    m_state = E_State.Run;
                    m_Animator.SetBool("Run", true);
                } 

                break;
            case E_State.Run:   //在追击状态下 会做什么
                //移动
                Move();

                //当到达攻击范围内条件满足。切换到攻击状态 
                if(IsAttack(m_Target))
                { 
                    m_state = E_State.Attack;
                   m_Animator.SetTrigger("Attack");
                    m_Animator.SetBool("Run", false);
                    m_Anget.isStopped = true;
                } 

                //达到了最大行动范围之外后 满足条件撤退
                if(IsMaxAciontRange(m_Target))
                {
                    m_state = E_State.GoBack;
                    m_CurPosition = m_OrgPos;
                }

                break;
            case E_State.GoBack:    //撤退
                //向原始点移动
                m_Anget.SetDestination(m_OrgPos);

                //如果达到目的地，条件满足切换到待机状态
                if(IsReacthPoint())
                {
                    m_state = E_State.Idle;
                }
                break;
            case E_State.Attack:    //在攻击状态下 会做什么

                //当目标不等于空
                if(m_Target!=null)
                {
                    //有间隔的去攻击目标
                    time += 1; 
                    if (time % 40 == 0) 
                    { 
                         transform.LookAt(m_Target.transform);
                         m_Animator.SetTrigger("Attack");
                    } 
                    
                    //当目标脱离了攻击范围的时候，条件满足会切入到追击状态,继续追击玩家
                    if (IsAttactRange(m_Target))
                    { 
                        m_state = E_State.Run;
                        m_Animator.SetBool("Run", true);
                    }  
                } 
                break;
        } 
    }

     
    public void Move()
    {   
        m_Anget.isStopped = false;
        m_Anget.SetDestination(m_Target.transform.position); 
    } 
    
    public bool IsSight(GameObject _target)
    {
        float dis = Vector3.Distance(transform.position, _target.transform.position);
        if (dis < m_Sight)
        {
            return true;
        }

        return false;
    } 

    public bool IsMaxAciontRange(GameObject _target)
    {
        float dis = Vector3.Distance(transform.position, _target.transform.position);
        if (dis > m_MaxActionRange)
        {
            return true;
        }

        return false;
    }
     
    public bool IsAttactRange(GameObject _target)
    {
        float dis = Vector3.Distance(transform.position, _target.transform.position);
        if (dis > m_AttackRange)
        {
            return true;
        }

        return false;
    }

     
    public bool IsAttack(GameObject _target)
    { 
        Vector3 selfV = transform.forward.normalized;
        Vector3 targeV = (_target.transform.position - transform.position).normalized;
        float result = Vector3.Dot(selfV, targeV);
        float result1 = Mathf.Acos(result);
        float angle = result1 * Mathf.Rad2Deg;
        float dis1 = Vector3.Distance(transform.position, _target.transform.position);

         
        if (dis1 <m_AttackRange)
        {
            return true;
        }
        return false;
     }

     
    public void EventAttack ()
    {
        if(m_Target!=null)
        {
            GameObject.Instantiate(BulletRes, m_FirePos.position, m_FirePos.rotation);
        }
    } 

    //在编辑器画线
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,m_Sight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_OrgPos,m_PatrolRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        ////如果碰到玩家
        //if (other.tag == "Player")
        //{
        //    other.GetComponent<PlayerStats>().TakeDamge(10);
        //}
    } 
}
