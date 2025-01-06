using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PinTuKuai : MonoBehaviour
{
    public bool CanMove;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void click()//点击记录当前拼图快
    {
        if (!CanMove)
        {
            if (PinTu.gameObjects[0] == null)
            {
                PinTu.gameObjects[0] = this.gameObject;
            }
            else
            {
                PinTu.gameObjects[1] = this.gameObject;
            }

        }
    }
}
