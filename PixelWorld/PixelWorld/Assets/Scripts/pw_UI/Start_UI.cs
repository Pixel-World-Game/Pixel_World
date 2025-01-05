using UnityEngine;
using UnityEngine.UI;

namespace pw_UI{
    public class Start_UI : MonoBehaviour{
        // We will create an instance of Game_List here in pure script mode.
        private GameObject panelObj;
        private GameObject gameTitleObj;
        private GameObject startButtonObj;
        private GameObject quitButtonObj;

        private Font customFont;

        // References to the Game_List instance we will create at runtime
        private GameObject gameListObj;
        private Game_List gameListUI;

        private void Start(){
            Debug.Log("Start_UI component initialized. Creating start menu UI...");

            // Try to load custom font from Resources/Fonts
            customFont = Resources.Load<Font>("Fonts/MinecraftCHMC");
            if (customFont == null)
                Debug.LogWarning(
                    "Could not load MinecraftCHMC.ttf from Resources/Fonts! Falling back to default font.");

            // 1. Create a fullscreen panel (semi-transparent background)
            panelObj = new GameObject("StartPanel");
            panelObj.transform.SetParent(transform, false);

            var panelRect = panelObj.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0);
            panelRect.anchorMax = new Vector2(1, 1);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            var panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.5f);

            // 2. Create the game title text
            gameTitleObj = CreateGameTitle(panelObj.transform, "Pixel World");

            // 3. Create "Start Game" button
            startButtonObj = CreateButton(
                panelObj.transform,
                "StartButton",
                "Start",
                new Vector2(0, 50),
                OnStartButtonClicked
            );

            // 4. Create "Quit" button
            quitButtonObj = CreateButton(
                panelObj.transform,
                "QuitButton",
                "Quit",
                new Vector2(0, -50),
                OnQuitButtonClicked
            );

            Debug.Log("Start menu UI creation completed.");

            // 5. Create a new GameObject for the Game_List UI
            gameListObj = new GameObject("GameListManager");
            // Add the Game_List component
            gameListUI = gameListObj.AddComponent<Game_List>();

            // By default, we make this UI inactive until it's needed
            gameListObj.SetActive(false);

            Debug.Log("GameListManager created, attached Game_List component, and set inactive by default.");
        }

        // Public method to show the UI
        public void ShowStartUI(){
            gameObject.SetActive(true);
            Debug.Log("Start UI is now visible.");
        }

        // When "Start" button is clicked
        private void OnStartButtonClicked(){
            Debug.Log("Start Game button clicked!");

            // Hide the current Start UI
            gameObject.SetActive(false);

            // Activate the GameList UI we just created
            gameListObj.SetActive(true);

            // If the component exists, call its ShowGameList method
            if (gameListUI != null)
                gameListUI.ShowGameList();
            else
                Debug.LogWarning("Game_List script is missing on the created GameListManager object!");
        }

        // When "Quit" button is clicked
        private void OnQuitButtonClicked(){
            Debug.Log("Quit Game button clicked!");
            Application.Quit();
        }

        // Helper: Create a button with pure script
        private GameObject CreateButton(
            Transform parent,
            string buttonName,
            string buttonText,
            Vector2 anchoredPosition,
            UnityEngine.Events.UnityAction onClickAction
        ){
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

            var textComponent = textObj.AddComponent<Text>();
            textComponent.text = buttonText;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.black;
            textComponent.font = customFont != null
                ? customFont
                : Resources.GetBuiltinResource<Font>("Arial.ttf");

            textComponent.fontSize = 36;

            return buttonObj;
        }

        // Helper: Create the game title text
        private GameObject CreateGameTitle(Transform parent, string titleText){
            var titleObj = new GameObject("GameTitle");
            titleObj.transform.SetParent(parent, false);

            var rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.anchoredPosition = new Vector2(0, -30);
            rectTransform.sizeDelta = new Vector2(400, 100);

            var textComponent = titleObj.AddComponent<Text>();
            textComponent.text = titleText;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;
            textComponent.fontSize = 72;
            textComponent.font = customFont != null
                ? customFont
                : Resources.GetBuiltinResource<Font>("Arial.ttf");

            return titleObj;
        }
    }
}