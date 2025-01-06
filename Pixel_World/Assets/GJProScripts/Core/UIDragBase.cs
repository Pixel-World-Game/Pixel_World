using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//拖拽基类
public class UIDragBase :  MonoBehaviour ,IDragHandler,IEndDragHandler
{
    public GameObject WinUI;
    //记录拼图得拼对次数
    public static int m_PinTuNum;

    //画布引用-用于拖拽UI时候需要改变父子级关系
    public Transform m_CanvasRoot;
    //背包根节点-用于拖拽UI时候需要改变父子级关系
    public Transform m_Root; 

    //是否按下
    public bool m_IsPress;

    //原始位置
    public Vector3 m_OrgPos;

    public TipPanel tip;

    public GameObject PinTuUI;

    private void Start()
    {
        m_OrgPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_IsPress = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UiFollowMouseMove();
        m_IsPress = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_IsPress = false;
    } 

    //UI跟随鼠标移动
    public void UiFollowMouseMove()
    {
        //鼠标拖拽UI坐标更新
        Vector2 outVec; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        gameObject.transform.parent as RectTransform
        ,Input.mousePosition,null,out outVec); 
        gameObject.transform.localPosition = outVec;  
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!m_IsPress)
        {
            //是否碰撞
            if (collision.gameObject.tag == "SuiPian")
            {
                Debug.Log("碰撞到碎片曹");
                float dis = Vector3.Distance(transform.position, collision.gameObject.transform.position);
                Debug.Log("距离:" + dis);
                if (dis <= 15.5f)
                {
                    if (GetComponent<SuiPianState>().m_Id == collision.gameObject.GetComponent<SuiPianState>().m_Id)
                    {
                        //如果比较靠近曹
                        collision.gameObject.GetComponent<Image>().enabled = true;
                        //collision.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
                        m_PinTuNum++;
                        if(m_PinTuNum>=9)
                        {
                            Debug.Log("工程");
                            tip.SetTip("需要解救的动物在一座废弃的工厂里!我们现在就出发吧",()=> {
                                WinUI.SetActive(true);
                                PinTuUI.SetActive(false);
                            } 
                                ); 
                        }
                        GameObject.Destroy(gameObject);
                    } 
                    else
                    {
                        transform.position = m_OrgPos;
                    }
                } 
                else
                {
                    transform.position = m_OrgPos;
                }
            }
            else
            {
                transform.position = m_OrgPos;
            }
        } 
    } 
    
}
