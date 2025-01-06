using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTu : MonoBehaviour
{
    public static GameObject[] gameObjects=new GameObject[2];
    public Vector3[] All = new Vector3[9];
    public GameObject Win;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            All[i] = this.transform.GetChild(i).transform.position;
        }
        for (int i = 0; i < 9; i++)//随机打乱拼图顺序
        {
            int temp = Random.Range(0, 9);
            Vector3 vector31 = this.transform.GetChild(i).transform.position;
            this.transform.GetChild(i).transform.position = this.transform.GetChild(temp).transform.position;
            this.transform.GetChild(temp).transform.position = vector31;
        }
        Win.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObjects[0] != null && gameObjects[1] != null)//选择两个图片，交换位置
        {
            if (gameObjects[0].GetComponent<RectTransform>().anchoredPosition.x + 268 == gameObjects[1].GetComponent<RectTransform>().anchoredPosition.x
                || gameObjects[0].GetComponent<RectTransform>().anchoredPosition.x - 268 == gameObjects[1].GetComponent<RectTransform>().anchoredPosition.x
                || gameObjects[0].GetComponent<RectTransform>().anchoredPosition.y + 205 == gameObjects[1].GetComponent<RectTransform>().anchoredPosition.y
                || gameObjects[0].GetComponent<RectTransform>().anchoredPosition.y - 205 == gameObjects[1].GetComponent<RectTransform>().anchoredPosition.y)
            {
                Vector3 vector31 = gameObjects[0].transform.position;
                gameObjects[0].transform.position = gameObjects[1].transform.position;
                gameObjects[1].transform.position = vector31;
                gameObjects[0] = null;
                gameObjects[1] = null;
            }
            else
            {
                gameObjects[0] = null;
                gameObjects[1] = null;
            }
           
        }
        int count=0;
        for (int i = 0; i < 9; i++)//查看当前每个拼图位置
        {
            if (All[i] == this.transform.GetChild(i).transform.position)
            {
                count += 1;
            }
           
        }
        if (count >= 9)//拼图成功
        {
            Win.SetActive(true);
            for (int i = 0; i < 9; i++)
            {
                this.transform.GetChild(i).transform.GetComponent<PinTuKuai>().CanMove = true;
                

            }
        }
    }
}
