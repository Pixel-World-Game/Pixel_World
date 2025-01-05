using UnityEngine;
using pw_UI;  // 命名空间，引用 Start_UI
using UnityEngine.EventSystems;  // 引入EventSystem/StandaloneInputModule所在的命名空间

public class Main : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Main Script Started!");
        
        CreateEventSystem();
        
        GameObject startUIObj = new GameObject("StartUIManager");
        Start_UI startUI = startUIObj.AddComponent<Start_UI>();

        var canvas = startUIObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        startUIObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        startUIObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        startUI.ShowStartUI();
    }

    private void CreateEventSystem()
    {
        EventSystem existingES = FindObjectOfType<EventSystem>();
        if (existingES == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<EventSystem>();
            esObj.AddComponent<StandaloneInputModule>();
            Debug.Log("EventSystem created via script.");
        }
        else
        {
            Debug.Log("EventSystem already exists in the scene.");
        }
    }
}