using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace pw_UI{
    public class New_Game : MonoBehaviour{
        [Header("Game Name Input")] public InputField gameNameInput;

        [Header("Seed Input")] public InputField seedInput;
        public int seedLengthLimit = 16;

        [Header("Start Button")] public Button startButton;

        // 统一将存档保存在 <persistentDataPath>/SavedGames/ 下
        private string gameFilesFolder;

        private void Awake(){
            // 1. 指定我们想存档的目录
            gameFilesFolder = Path.Combine(Application.persistentDataPath, "SavedGames");
            if (!Directory.Exists(gameFilesFolder)) Directory.CreateDirectory(gameFilesFolder);

            // 2. 按钮点击绑定
            if (startButton != null)
                startButton.onClick.AddListener(OnStartGameClicked);
            else
                Debug.LogWarning("Start Button is not assigned in the Inspector!");
        }

        // 当用户点击“Start”时执行
        private void OnStartGameClicked(){
            // 获取游戏名
            var gameName = gameNameInput != null ? gameNameInput.text : "UnnamedGame";

            // 获取种子
            var seed = seedInput != null ? seedInput.text : "";

            if (seed.Length > seedLengthLimit){
                Debug.LogWarning($"Seed length cannot exceed {seedLengthLimit} characters.");
                return;
            }

            var fileName = gameName.Replace(" ", "_") + "_GameData.txt";
            var filePath = Path.Combine(gameFilesFolder, fileName);

            var fileContent = $"GameName: {gameName}\nSeed: {seed}";

            try{
                File.WriteAllText(filePath, fileContent);
                Debug.Log($"Game file created at: {filePath}");
            }
            catch (IOException e){
                Debug.LogError($"Failed to write game file: {e.Message}");
                return;
            }

            Debug.Log($"Starting game with file: {filePath}");
            // Game.StartGame(filePath);
        }
    }
}