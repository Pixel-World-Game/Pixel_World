using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

// Game_List.cs
// This script shows a list of saved games, includes a search box, and a "New Game" button as the first item.

namespace pw_UI{
    public class Game_List : MonoBehaviour{
        [Header("Search Field")]
        // Reference to an InputField used for searching saved games
        public InputField searchField;

        [Header("Scroll Rect")]
        // Reference to a ScrollRect that contains the list of saved games
        public ScrollRect scrollRect;

        [Header("List Content")]
        // The content Transform inside the ScrollRect where each item (Button) will be placed
        public Transform listContent;

        [Header("List Item Prefab")]
        // A prefab or template for each item in the list (Button with a label)
        public GameObject listItemPrefab;

        // The path where saved games might be located
        private string gameFilesFolder;
        private List<string> allSavedGames = new();

        [Header("New Game UI")]
        // Reference to the New_Game script or the UI Manager for new game creation
        public New_Game newGameUI;

        private void Awake(){
            // Let's assume saved games are in <persistentDataPath>/Saves
            gameFilesFolder = Path.Combine(Application.persistentDataPath, "Saves");

            if (!Directory.Exists(gameFilesFolder)) Directory.CreateDirectory(gameFilesFolder);

            // If there's a search field, add a listener for text change
            if (searchField != null) searchField.onValueChanged.AddListener(OnSearchValueChanged);
        }

        // Call this method when showing the Game_List page
        public void ShowGameList(){
            Debug.Log("Game_List: Showing game list UI.");

            // Refresh the list of saved games
            LoadAllSavedGames();
            // Rebuild the UI list with no filter by default
            RebuildList("");
        }

        // Load all saved game files from the folder
        private void LoadAllSavedGames(){
            Debug.Log($"Searching for saved game files in: {gameFilesFolder}");

            allSavedGames.Clear();
            // We are looking for files with .pwdat extension
            var files = Directory.GetFiles(gameFilesFolder, "*.pwdat", SearchOption.TopDirectoryOnly);

            if (files.Length == 0) Debug.Log("No saved game files found. The list is empty.");

            foreach (var file in files) allSavedGames.Add(Path.GetFileName(file));
        }

        // Called when user types in the search field
        private void OnSearchValueChanged(string searchText){
            RebuildList(searchText);
        }

        // Rebuild the UI list
        private void RebuildList(string filter){
            if (listContent == null){
                Debug.Log("listContent is null.");
                return;
            }

            if (listItemPrefab == null){
                Debug.Log("listItemPrefab is null.");
                return;
            }

            // Clear existing items
            foreach (Transform child in listContent) Destroy(child.gameObject);

            // Always create "New Game" button at the top
            CreateNewGameButton();

            // Filter the saved files
            var foundAnyMatchingFile = false;
            if (allSavedGames == null){
                Debug.LogWarning("allSavedGames is null. No saved files to display.");
                return;
            }

            foreach (var saveFile in allSavedGames)
                if (string.IsNullOrEmpty(filter)
                    || saveFile.ToLower().Contains(filter.ToLower())){
                    CreateSaveFileButton(saveFile);
                    foundAnyMatchingFile = true;
                }

            // If no matching save files were found, do NOT remove "New Game" button;
            // just add a "no results" label below it
            if (!foundAnyMatchingFile) CreateNoResultsLabel();
        }

        // Helper method to show "No matching saved games" label
        private void CreateNoResultsLabel(){
            if (listItemPrefab == null){
                Debug.LogWarning("listItemPrefab is null. Creating a label from code instead.");
                // Fallback: purely create a Text object
                var fallbackLabel = new GameObject("NoResultsLabel_Fallback");
                fallbackLabel.transform.SetParent(listContent, false);

                var rectTransform = fallbackLabel.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(300, 60);

                var textComponent = fallbackLabel.AddComponent<Text>();
                textComponent.text = "No matching saved games found.";
                textComponent.alignment = TextAnchor.MiddleCenter;
                textComponent.color = Color.red;
                textComponent.font = Resources.Load<Font>("Fonts/MinecraftCHMC");
                textComponent.fontSize = 28;

                return;
            }

            var itemObj = Instantiate(listItemPrefab, listContent);
            itemObj.name = "NoResultsLabel";

            var btn = itemObj.GetComponent<Button>();
            if (btn != null) btn.interactable = false;

            var txt = itemObj.GetComponentInChildren<Text>();
            if (txt != null) txt.text = "No matching saved games found.";

            Debug.Log("Created 'NoResultsLabel' using listItemPrefab.");
        }

        private void CreateNewGameButton(){
            var itemObj = Instantiate(listItemPrefab, listContent);
            itemObj.name = "NewGameButtonItem";

            var btn = itemObj.GetComponent<Button>();
            var txt = itemObj.GetComponentInChildren<Text>();

            if (txt != null) txt.text = "New Game";

            btn.onClick.AddListener(() => {
                Debug.Log("New Game button clicked! Opening New_Game UI...");
                gameObject.SetActive(false);

                if (newGameUI != null)
                    newGameUI.gameObject.SetActive(true);
                else
                    Debug.LogWarning("No reference to New_Game UI!");
            });
        }

        private void CreateSaveFileButton(string saveFileName){
            var itemObj = Instantiate(listItemPrefab, listContent);
            itemObj.name = "GameItem_" + saveFileName;

            var btn = itemObj.GetComponent<Button>();
            var txt = itemObj.GetComponentInChildren<Text>();
            if (txt != null) txt.text = saveFileName;

            btn.onClick.AddListener(() => {
                Debug.Log($"Selected saved game file: {saveFileName}");
                // TODO: load game logic
                // string fullPath = Path.Combine(gameFilesFolder, saveFileName);
                // Game.LoadGame(fullPath);
            });
        }
    }
}