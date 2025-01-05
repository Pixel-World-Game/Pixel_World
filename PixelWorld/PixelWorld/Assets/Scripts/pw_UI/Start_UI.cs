using UnityEngine;
using UnityEngine.UI;

namespace pw_UI
{
    public class Start_UI : MonoBehaviour
    {
        // We will create an instance of Game_List here in pure script mode.

        private GameObject mainCanvasObj;
        private Font customFont;

        private GameObject startPanel;
        private GameObject gameTitleObj;
        private GameObject startButtonObj;
        private GameObject quitButtonObj;

        // Reference to the Game_List instance we create at runtime
        private GameObject gameListObj;
        private Game_List gameListUI;

        private void Start()
        {
            Debug.Log("Start_UI: Initializing UI by code...");

            // 1. Create a main Canvas so all UI is visible
            CreateMainCanvas();

            // 2. Load custom font (MinecraftCHMC) from Resources/Fonts
            customFont = Resources.Load<Font>("Fonts/MinecraftCHMC");
            if (customFont == null)
            {
                Debug.LogWarning("Could not load MinecraftCHMC.ttf from Resources/Fonts! Using no fallback font.");
            }

            // 3. Create a semi-transparent panel to fill the Canvas
            CreateStartPanel();

            // 4. Create a game title text
            gameTitleObj = CreateGameTitle(startPanel.transform, "Pixel World");

            // 5. Create "Start" and "Quit" buttons
            startButtonObj = CreateButton(
                startPanel.transform,
                "StartButton",
                "Start",
                new Vector2(0, 50),
                OnStartButtonClicked
            );

            quitButtonObj = CreateButton(
                startPanel.transform,
                "QuitButton",
                "Quit",
                new Vector2(0, -50),
                OnQuitButtonClicked
            );

            Debug.Log("Start menu UI creation completed.");

            // 6. Create a new GameObject for Game_List UI
            // and add Game_List component. Inactive by default.
            gameListObj = new GameObject("GameListManager");
            gameListUI = gameListObj.AddComponent<Game_List>();
            gameListObj.SetActive(false);

            Debug.Log("GameListManager created, attached Game_List, inactive by default.");
        }

        // Public method to show Start UI
        public void ShowStartUI()
        {
            if (mainCanvasObj != null)
            {
                mainCanvasObj.SetActive(true);
            }
            Debug.Log("Start UI is now visible.");
        }

        // When "Start" button is clicked
        private void OnStartButtonClicked()
        {
            Debug.Log("Start Game button clicked!");

            // Hide this UI's Canvas
            if (mainCanvasObj != null)
            {
                mainCanvasObj.SetActive(false);
            }

            // Activate GameList UI
            gameListObj.SetActive(true);

            if (gameListUI != null)
            {
                gameListUI.ShowGameList();
            }
            else
            {
                Debug.LogWarning("Game_List script not found on the created GameListManager object!");
            }
        }

        // When "Quit" button is clicked
        private void OnQuitButtonClicked()
        {
            Debug.Log("Quit Game button clicked!");
            Application.Quit();
        }

        // -----------------------------------------
        // Helper methods to create UI in pure code
        // -----------------------------------------

        // Create a main Canvas to hold all UI
        private void CreateMainCanvas()
        {
            mainCanvasObj = new GameObject("StartUICanvas");
            var canvas = mainCanvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            mainCanvasObj.AddComponent<CanvasScaler>();
            mainCanvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create a semi-transparent panel filling the canvas
        private void CreateStartPanel()
        {
            startPanel = new GameObject("StartPanel");
            startPanel.transform.SetParent(mainCanvasObj.transform, false);

            var panelRect = startPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            var panelImage = startPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.5f);
        }

        // Create a button with text
        private GameObject CreateButton(
            Transform parent,
            string buttonName,
            string buttonText,
            Vector2 anchoredPosition,
            UnityEngine.Events.UnityAction onClickAction
        )
        {
            var buttonObj = new GameObject(buttonName);
            buttonObj.transform.SetParent(parent, false);

            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 60);
            rectTransform.anchoredPosition = anchoredPosition;

            var btnImage = buttonObj.AddComponent<Image>();
            btnImage.color = Color.white;

            var button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(onClickAction);

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var textComp = textObj.AddComponent<Text>();
            textComp.text = buttonText;
            textComp.alignment = TextAnchor.MiddleCenter;
            textComp.color = Color.black;
            textComp.font = customFont;
            textComp.fontSize = 36;

            return buttonObj;
        }

        // Create the game title at the top
        private GameObject CreateGameTitle(Transform parent, string titleText)
        {
            var titleObj = new GameObject("GameTitle");
            titleObj.transform.SetParent(parent, false);

            var rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.anchoredPosition = new Vector2(0, -30);
            rectTransform.sizeDelta = new Vector2(400, 100);

            var textComp = titleObj.AddComponent<Text>();
            textComp.text = titleText;
            textComp.alignment = TextAnchor.MiddleCenter;
            textComp.color = Color.white;
            textComp.fontSize = 72;
            textComp.font = customFont;

            return titleObj;
        }
    }
}
