using UnityEngine;
using pw_UI; // Namespace referencing Start_UI
using UnityEngine.EventSystems; // Includes EventSystem/StandaloneInputModule

public class Main : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Main Script Started!");

        CreateEventSystem();

        // Create a GameObject for the Start UI
        var startUIObj = new GameObject("StartUIManager");
        var startUI = startUIObj.AddComponent<Start_UI>();

        // Set up Canvas components
        var canvas = startUIObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        startUIObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        startUIObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        startUI.ShowStartUI();
    }

    private void CreateEventSystem()
    {
        var existingES = FindObjectOfType<EventSystem>();
        if (existingES == null)
        {
            // Create a new EventSystem
            var esObj = new GameObject("EventSystem");
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