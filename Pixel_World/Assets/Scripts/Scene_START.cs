using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// A scene controller that creates a start menu entirely via code.
/// </summary>
public class Scene_START : MonoBehaviour
{
    private Canvas startMenuCanvas;
    private Button newGameButton;
    private Button loadGameButton;

    /// <summary>
    /// This method is called automatically by Unity when the scene is loaded.
    /// We create the UI elements (Canvas, Buttons, EventSystem) at runtime.
    /// </summary>
    private void Start()
    {
        CreateEventSystem();
        CreateCanvas();
        CreateButtons();
    }

    /// <summary>
    /// Creates an EventSystem object so that UI interactions can work.
    /// </summary>
    private void CreateEventSystem()
    {
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.AddComponent<EventSystem>();
        eventSystemObj.AddComponent<StandaloneInputModule>();
    }

    /// <summary>
    /// Creates a Canvas in Overlay mode, making the UI visible on the screen.
    /// </summary>
    private void CreateCanvas()
    {
        // Create a new GameObject for the Canvas
        GameObject canvasObject = new GameObject("StartMenuCanvas");
        startMenuCanvas = canvasObject.AddComponent<Canvas>();
        startMenuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Optionally add a CanvasScaler if you want to handle different resolutions gracefully
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Add a GraphicRaycaster so the canvas can detect UI events
        canvasObject.AddComponent<GraphicRaycaster>();
    }

    /// <summary>
    /// Creates two buttons: "New Game" and "Load Game".
    /// </summary>
    private void CreateButtons()
    {
        // Create "New Game" button
        newGameButton = CreateButton(
            "NewGameButton", 
            "New Game", 
            new Vector2(0, 50)  // anchoredPosition
        );
        newGameButton.onClick.AddListener(OnNewGameClicked);

        // Create "Load Game" button
        loadGameButton = CreateButton(
            "LoadGameButton", 
            "Load Game", 
            new Vector2(0, -50) // anchoredPosition
        );
        loadGameButton.onClick.AddListener(OnLoadGameClicked);
    }

    /// <summary>
    /// Helper method to create a button with text, given a name, label, and anchored position.
    /// </summary>
    private Button CreateButton(string objName, string buttonText, Vector2 anchoredPos)
    {
        // Create the Button object
        GameObject buttonObj = new GameObject(objName);
        buttonObj.transform.SetParent(startMenuCanvas.transform, false);

        // Add RectTransform and configure size/position
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 60);
        rectTransform.anchoredPosition = anchoredPos;

        // Add an Image component for the Button background
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 1f); // light gray

        // Add the actual Button component
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = Color.white;
        button.colors = colors;

        // Create Text child object for the button label
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(200, 60);

        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Use a built-in font
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;

        return button;
    }

    /// <summary>
    /// Callback for the "New Game" button click.
    /// </summary>
    private void OnNewGameClicked()
    {
        Debug.Log("Starting a new game...");
        // TODO:
        // 1) Create a new Environment (e.g., with a 32-char hash).
        // 2) Transition to your main gameplay scene if necessary.
        // SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Callback for the "Load Game" button click.
    /// </summary>
    private void OnLoadGameClicked()
    {
        Debug.Log("Loading a saved game...");
        // TODO:
        // 1) Prompt the user to choose a save file or specify a default path.
        // 2) Load the Environment from that file.
        // 3) Transition to your main gameplay scene.
        // SceneManager.LoadScene("GameScene");
    }
}
