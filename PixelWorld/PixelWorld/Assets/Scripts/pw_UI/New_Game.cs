using UnityEngine;
using UnityEngine.UI;
using System.IO;  // For file operations

namespace pw_UI
{
    // New_Game.cs
    // This script collects game initialization data and starts a new game.
    public class New_Game : MonoBehaviour
    {
        [Header("Game Name Input")]
        // Reference to the InputField for the game name
        public InputField gameNameInput;
        
        [Header("Seed Input")]
        // Reference to the InputField for the seed
        public InputField seedInput;

        // Limit for seed length
        public int seedLengthLimit = 16;

        [Header("Start Button")]
        // Button to start the game
        public Button startButton;

        // We'll store our game files in this folder
        private string gameFilesFolder;

        void Awake()
        {
            // Construct the path to store game files
            // For example: <persistentDataPath>/SavedGames
            gameFilesFolder = Path.Combine(Application.persistentDataPath, "SavedGames");

            if (!Directory.Exists(gameFilesFolder))
            {
                Directory.CreateDirectory(gameFilesFolder);
            }

            // If the button is assigned, add listener
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartGameClicked);
            }
            else
            {
                Debug.LogWarning("Start Button is not assigned in the Inspector!");
            }
        }

        // Called when the user clicks the "Start" button
        private void OnStartGameClicked()
        {
            // 1. Get the game name
            string gameName = (gameNameInput != null) ? gameNameInput.text : "UnnamedGame";

            // 2. Get the seed
            string seed = (seedInput != null) ? seedInput.text : "";

            // 3. Check seed length
            if (seed.Length > seedLengthLimit)
            {
                Debug.LogWarning($"Seed length cannot exceed {seedLengthLimit} characters.");
                // You could show a UI warning or just return
                return;
            }

            // 4. Create a game file
            // We'll keep it simple and just store plain text
            string fileName = gameName.Replace(" ", "_") + "_GameData.dat";
            string filePath = Path.Combine(gameFilesFolder, fileName);

            string fileContent = $"GameName: {gameName}\nSeed: {seed}";
            try
            {
                File.WriteAllText(filePath, fileContent);
                Debug.Log($"Game file created: {filePath}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to write game file: {e.Message}");
                return;
            }

            // 5. Call Game.cs to start the game
            // For example: Game.StartGame(filePath);
            // Or load a new scene that references this data
            // e.g. SceneManager.LoadScene("GameScene");
            Debug.Log($"Starting game with file: {filePath}");
            // Game.StartGame(filePath); // Example usage
        }
    }
}
