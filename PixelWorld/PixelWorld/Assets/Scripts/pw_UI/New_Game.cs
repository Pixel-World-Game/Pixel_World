using UnityEngine;
using UnityEngine.UI;
using System.IO;
using pw_SaveManage; // For pwdat, GameSaveData, etc.
using pw_Game;       // For Game class

namespace pw_UI
{
    public class New_Game : MonoBehaviour
    {
        private GameObject mainCanvasObj;
        private Font customFont;

        private InputField gameNameInput;
        private InputField seedInput;

        private string savesFolder;

        private void Start()
        {
            Debug.Log("New_Game UI: Initializing...");

            // 1. Create main Canvas
            CreateMainCanvas();

            // 2. Load font
            customFont = Resources.Load<Font>("Fonts/MinecraftCHMC");
            if (customFont == null)
            {
                Debug.LogWarning("Could not load MinecraftCHMC.ttf from Resources/Fonts! Using default font.");
            }

            // 3. Build UI input fields
            BuildInputFields();

            // 4. Build Start button
            BuildStartButton();

            // 5. Prepare saves folder
            savesFolder = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(savesFolder))
            {
                Directory.CreateDirectory(savesFolder);
            }

            Debug.Log("New_Game UI: Ready.");
        }

        private void CreateMainCanvas()
        {
            mainCanvasObj = new GameObject("NewGameCanvas_MainUI");
            var canvas = mainCanvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            mainCanvasObj.AddComponent<CanvasScaler>();
            mainCanvasObj.AddComponent<GraphicRaycaster>();
        }

        private void BuildInputFields()
        {
            var panel = new GameObject("InputFieldsPanel");
            panel.transform.SetParent(mainCanvasObj.transform, false);

            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(400, 200);
            panelRect.anchoredPosition = Vector2.zero;

            var panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0.9f, 0.9f, 0.9f);

            // -- Game Name --
            var nameObj = CreateLabeledInput(panel.transform, "Game Name:", out InputField nameInput);
            nameObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
            gameNameInput = nameInput;

            // -- Seed --
            var seedObj = CreateLabeledInput(panel.transform, "Seed (<=16 chars):", out InputField seedInputField);
            seedObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            seedInputField.characterLimit = 16;
            seedInput = seedInputField;
        }

        private void BuildStartButton()
        {
            var startButtonObj = new GameObject("StartButton");
            startButtonObj.transform.SetParent(mainCanvasObj.transform, false);

            var btnRect = startButtonObj.AddComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(160, 40);
            btnRect.anchoredPosition = new Vector2(0, -140);

            var btnImage = startButtonObj.AddComponent<Image>();
            btnImage.color = Color.white;

            var button = startButtonObj.AddComponent<Button>();
            button.onClick.AddListener(OnStartButtonClicked);

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(startButtonObj.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;

            var textComp = textObj.AddComponent<Text>();
            textComp.text = "Start";
            textComp.alignment = TextAnchor.MiddleCenter;
            textComp.fontSize = 24;
            textComp.color = Color.black;
            textComp.font = customFont;
        }

        private GameObject CreateLabeledInput(Transform parent, string labelText, out InputField inputField)
        {
            var container = new GameObject("LabeledInput");
            container.transform.SetParent(parent, false);

            var cRect = container.AddComponent<RectTransform>();
            cRect.sizeDelta = new Vector2(380, 40);

            // Label
            var labelObj = new GameObject("Label");
            labelObj.transform.SetParent(container.transform, false);

            var labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0.5f);
            labelRect.anchorMax = new Vector2(0, 0.5f);
            labelRect.sizeDelta = new Vector2(160, 40);
            labelRect.anchoredPosition = new Vector2(80, 0);

            var labelTextComp = labelObj.AddComponent<Text>();
            labelTextComp.text = labelText;
            labelTextComp.font = customFont;
            labelTextComp.fontSize = 20;
            labelTextComp.color = Color.black;
            labelTextComp.alignment = TextAnchor.MiddleRight;

            // Input Field
            var inputObj = new GameObject("InputField");
            inputObj.transform.SetParent(container.transform, false);

            var iRect = inputObj.AddComponent<RectTransform>();
            iRect.anchorMin = new Vector2(1, 0.5f);
            iRect.anchorMax = new Vector2(1, 0.5f);
            iRect.sizeDelta = new Vector2(160, 30);
            iRect.anchoredPosition = new Vector2(-90, 0);

            var iImg = inputObj.AddComponent<Image>();
            iImg.color = Color.white;

            inputField = inputObj.AddComponent<InputField>();

            // Text component
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(inputObj.transform, false);

            var txtRect = textObj.AddComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;

            var txtComp = textObj.AddComponent<Text>();
            txtComp.font = customFont;
            txtComp.fontSize = 20;
            txtComp.color = Color.black;
            txtComp.alignment = TextAnchor.MiddleLeft;

            inputField.textComponent = txtComp;

            // Placeholder
            var placeholderObj = new GameObject("Placeholder");
            placeholderObj.transform.SetParent(inputObj.transform, false);

            var phRect = placeholderObj.AddComponent<RectTransform>();
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;

            var phText = placeholderObj.AddComponent<Text>();
            phText.font = customFont;
            phText.fontSize = 20;
            phText.color = new Color(0.6f, 0.6f, 0.6f);
            phText.alignment = TextAnchor.MiddleLeft;

            inputField.placeholder = phText;

            return container;
        }

        private void OnStartButtonClicked()
        {
            // Read user input
            var gameName = gameNameInput != null ? gameNameInput.text : "NewGame";
            var seedStr = seedInput != null ? seedInput.text : "0000";

            // Build file path
            string fileName = $"{gameName}_{seedStr}.pwdat";
            string filePath = Path.Combine(savesFolder, fileName);

            // 1. Create new save data using pwdat's helper
            var newGameSaveData = pwdat.CreateNewGameSaveData(gameName, seedStr);

            // 2. Save it to .pwdat
            pwdat.SavePwdat(filePath, newGameSaveData);

            Debug.Log($"New game .pwdat created at: {filePath}");

            // 3. Create a new Game instance and load from file
            var gameObj = new GameObject("GameManager");
            var gameInstance = gameObj.AddComponent<Game>();
            gameInstance.LoadGameFromFile(filePath);

            // 4. Hide current UI
            if (mainCanvasObj != null)
            {
                mainCanvasObj.SetActive(false);
            }
        }
    }
}
