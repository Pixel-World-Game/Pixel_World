using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using pw_Game;

namespace pw_UI{
    public class Game_List : MonoBehaviour{
        private string savesFolder;
        private List<string> allSavedGames = new();

        private GameObject mainCanvasObj;
        private Font customFont;

        private InputField searchInput;
        private ScrollRect scrollRect;
        private Transform listContent;

        // Optional: if you want to show a separate New_Game UI
        public GameObject newGameObj;

        private void Awake(){
            // 1. Create Canvas
            CreateMainCanvas();

            // 2. Load font
            customFont = Resources.Load<Font>("Fonts/MinecraftCHMC");
            if (customFont == null)
                Debug.LogWarning("MinecraftCHMC.ttf not found in Resources/Fonts.");

            // 3. Build UI: top search, middle list, bottom button
            var searchBar = BuildSearchBar();
            searchInput = searchBar.GetComponentInChildren<InputField>();
            if (searchInput != null)
                searchInput.onValueChanged.AddListener(OnSearchValueChanged);

            var scrollPanel = BuildScrollList();
            scrollRect = scrollPanel.GetComponentInChildren<ScrollRect>();
            if (scrollRect != null)
                listContent = scrollRect.content;

            BuildNewGameButton();

            // 4. Folder for .pwdat
            savesFolder = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(savesFolder))
                Directory.CreateDirectory(savesFolder);

            // Hide initially
            mainCanvasObj.SetActive(false);
        }

        public void ShowGameList(){
            mainCanvasObj.SetActive(true);
            LoadAllSavedGames();
            RebuildList("");
            if (searchInput != null)
                searchInput.text = "";
        }

        private void LoadAllSavedGames(){
            Debug.Log($"Scanning .pwdat in {savesFolder}");
            allSavedGames.Clear();
            var files = Directory.GetFiles(savesFolder, "*.pwdat", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                Debug.Log("No saved games found.");

            foreach (var f in files)
                allSavedGames.Add(Path.GetFileName(f));
        }

        private void OnSearchValueChanged(string keyword){
            RebuildList(keyword);
        }

        private void RebuildList(string keyword){
            if (listContent == null) return;
            foreach (Transform child in listContent)
                Destroy(child.gameObject);

            var foundAny = false;
            foreach (var file in allSavedGames)
                if (string.IsNullOrEmpty(keyword) || file.ToLower().Contains(keyword.ToLower())){
                    CreateSaveFileItem(file);
                    foundAny = true;
                }

            if (!foundAny)
                CreateNoResultsLabel();
        }

        // -------------------------
        // UI building
        // -------------------------
        private void CreateMainCanvas(){
            mainCanvasObj = new GameObject("GameListCanvas");
            var canvas = mainCanvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvasObj.AddComponent<CanvasScaler>();
            mainCanvasObj.AddComponent<GraphicRaycaster>();
        }

        private GameObject BuildSearchBar(){
            var bar = new GameObject("SearchBar");
            bar.transform.SetParent(mainCanvasObj.transform, false);

            var r = bar.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(0, 1);
            r.anchorMax = new Vector2(1, 1);
            r.pivot = new Vector2(0.5f, 1f);
            r.sizeDelta = new Vector2(0, 60);

            var bg = bar.AddComponent<Image>();
            bg.color = new Color(0.8f, 0.8f, 0.8f);

            var inputObj = new GameObject("SearchField");
            inputObj.transform.SetParent(bar.transform, false);

            var iRect = inputObj.AddComponent<RectTransform>();
            iRect.anchorMin = Vector2.zero;
            iRect.anchorMax = Vector2.one;
            iRect.offsetMin = new Vector2(10, 10);
            iRect.offsetMax = new Vector2(-10, -10);

            var iImg = inputObj.AddComponent<Image>();
            iImg.color = Color.white;

            var input = inputObj.AddComponent<InputField>();

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(inputObj.transform, false);

            var tRect = textObj.AddComponent<RectTransform>();
            tRect.anchorMin = Vector2.zero;
            tRect.anchorMax = Vector2.one;

            var tComp = textObj.AddComponent<Text>();
            tComp.font = customFont;
            tComp.fontSize = 24;
            tComp.color = Color.black;
            tComp.alignment = TextAnchor.MiddleLeft;

            input.textComponent = tComp;

            // Placeholder
            var phObj = new GameObject("Placeholder");
            phObj.transform.SetParent(inputObj.transform, false);
            var phRect = phObj.AddComponent<RectTransform>();
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;

            var phText = phObj.AddComponent<Text>();
            phText.text = "Search...";
            phText.font = customFont;
            phText.fontSize = 24;
            phText.color = new Color(0.5f, 0.5f, 0.5f);
            phText.alignment = TextAnchor.MiddleLeft;

            input.placeholder = phText;

            return bar;
        }

        private GameObject BuildScrollList(){
            var listPanel = new GameObject("ListPanel");
            listPanel.transform.SetParent(mainCanvasObj.transform, false);

            var lpRect = listPanel.AddComponent<RectTransform>();
            lpRect.anchorMin = new Vector2(0, 0);
            lpRect.anchorMax = new Vector2(1, 1);
            lpRect.offsetMin = new Vector2(0, 60);
            lpRect.offsetMax = new Vector2(0, -60);

            var lpImg = listPanel.AddComponent<Image>();
            lpImg.color = new Color(0.9f, 0.9f, 0.9f);

            var scrollObj = new GameObject("ScrollView");
            scrollObj.transform.SetParent(listPanel.transform, false);

            var srRect = scrollObj.AddComponent<RectTransform>();
            srRect.anchorMin = Vector2.zero;
            srRect.anchorMax = Vector2.one;

            var sr = scrollObj.AddComponent<ScrollRect>();
            sr.horizontal = false;

            var vpObj = new GameObject("Viewport");
            vpObj.transform.SetParent(scrollObj.transform, false);

            var vpRect = vpObj.AddComponent<RectTransform>();
            vpRect.anchorMin = Vector2.zero;
            vpRect.anchorMax = Vector2.one;

            var vpMask = vpObj.AddComponent<Mask>();
            vpMask.showMaskGraphic = false;

            var vpImg = vpObj.AddComponent<Image>();
            vpImg.color = new Color(1, 1, 1, 0.1f);

            sr.viewport = vpRect;

            var contentObj = new GameObject("Content");
            contentObj.transform.SetParent(vpObj.transform, false);

            var cRect = contentObj.AddComponent<RectTransform>();
            cRect.anchorMin = new Vector2(0, 1);
            cRect.anchorMax = new Vector2(1, 1);
            cRect.pivot = new Vector2(0.5f, 1f);
            cRect.sizeDelta = new Vector2(0, 600);

            sr.content = cRect;

            return listPanel;
        }

        private void BuildNewGameButton(){
            var bottomPanel = new GameObject("NewGamePanel");
            bottomPanel.transform.SetParent(mainCanvasObj.transform, false);

            var bpRect = bottomPanel.AddComponent<RectTransform>();
            bpRect.anchorMin = new Vector2(0, 0);
            bpRect.anchorMax = new Vector2(1, 0);
            bpRect.pivot = new Vector2(0.5f, 0f);
            bpRect.sizeDelta = new Vector2(0, 60);

            var bpImg = bottomPanel.AddComponent<Image>();
            bpImg.color = new Color(0.8f, 0.8f, 0.8f);

            var newGameButton = new GameObject("NewGameButton");
            newGameButton.transform.SetParent(bottomPanel.transform, false);

            var btnRect = newGameButton.AddComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(160, 40);

            var btnImg = newGameButton.AddComponent<Image>();
            btnImg.color = Color.white;

            var button = newGameButton.AddComponent<Button>();
            button.onClick.AddListener(OnNewGameButtonClicked);

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(newGameButton.transform, false);

            var tRect = textObj.AddComponent<RectTransform>();
            tRect.anchorMin = Vector2.zero;
            tRect.anchorMax = Vector2.one;

            var tComp = textObj.AddComponent<Text>();
            tComp.text = "New Game";
            tComp.font = customFont;
            tComp.fontSize = 24;
            tComp.alignment = TextAnchor.MiddleCenter;
            tComp.color = Color.black;
        }

        private void CreateSaveFileItem(string fileName)
        {
            if (listContent == null) return;

            var itemObj = new GameObject($"Item_{fileName}");
            itemObj.transform.SetParent(listContent, false);

            var rect = itemObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 60);

            var bg = itemObj.AddComponent<Image>();
            bg.color = Color.white;

            var btn = itemObj.AddComponent<Button>();
            btn.onClick.AddListener(() => {
                Debug.Log($"Clicked: {fileName}");

                // 1. 构建绝对路径或相对路径
                //    假设 .pwdat 存储在 Application.persistentDataPath + "/Saves/"
                //    并且 fileName 就是 "{存档名}.pwdat" 
                string savesFolder = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
                string filePath = System.IO.Path.Combine(savesFolder, fileName);
        
                // 2. 获取或找到 Game 脚本引用
                //    假设场景中有一个 "GameManager" GameObject 挂载了 Game.cs
                //    或者你可以把 Game 做成单例，或 FindObjectOfType<Game>() 均可
                var gameObj = GameObject.Find("GameManager");
                if (gameObj != null)
                {
                    var gameScript = gameObj.GetComponent<Game>();
                    if (gameScript != null)
                    {
                        gameScript.LoadGameFromFile(filePath);
                    }
                    else
                    {
                        Debug.LogWarning("Game script not found on GameManager object.");
                    }
                }
                else
                {
                    Debug.LogWarning("GameManager object not found in scene.");
                }
            });

            var textObj = new GameObject("ItemText");
            textObj.transform.SetParent(itemObj.transform, false);

            var txtRect = textObj.AddComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;

            var txtComp = textObj.AddComponent<Text>();
            txtComp.text = fileName;
            txtComp.font = customFont;
            txtComp.fontSize = 24;
            txtComp.alignment = TextAnchor.MiddleCenter;
            txtComp.color = Color.black;
        }

        private void CreateNoResultsLabel(){
            if (listContent == null) return;

            var labelObj = new GameObject("NoResults");
            labelObj.transform.SetParent(listContent, false);

            var rect = labelObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 60);

            var txt = labelObj.AddComponent<Text>();
            txt.text = "No matching saved games found.";
            txt.font = customFont;
            txt.fontSize = 24;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.red;
        }

        private void OnNewGameButtonClicked(){
            Debug.Log("New Game button clicked -> Hide this list UI, show New_Game UI.");

            // Hide current UI
            if (mainCanvasObj) mainCanvasObj.SetActive(false);

            var newGameCanvas = new GameObject("NewGameCanvas");
            var canvas = newGameCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            newGameCanvas.AddComponent<CanvasScaler>();
            newGameCanvas.AddComponent<GraphicRaycaster>();

            var newGameUI = newGameCanvas.AddComponent<New_Game>();
            newGameCanvas.SetActive(true);
        }
    }
}