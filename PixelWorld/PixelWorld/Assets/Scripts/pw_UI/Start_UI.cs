using UnityEngine;
using UnityEngine.UI;

namespace pw_UI
{
    public class Start_UI : MonoBehaviour
    {

        private GameObject panelObj;
        private GameObject gameTitleObj;
        private GameObject startButtonObj;
        private GameObject quitButtonObj;

        private Font customFont;

        void Start()
        {
            Debug.Log("Start_UI component initialized. Creating start menu UI...");

            // 尝试从 Resources/Fonts 文件夹加载自定义字体
            customFont = Resources.Load<Font>("Fonts/MinecraftCHMC");
            if (customFont == null)
            {
                Debug.LogWarning("Could not load MinecraftCHMC.ttf from Resources/Fonts! Falling back to default font.");
            }

            // 1. 创建一个全屏面板（半透明背景）
            panelObj = new GameObject("StartPanel");
            panelObj.transform.SetParent(this.transform, false);

            RectTransform panelRect = panelObj.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0);
            panelRect.anchorMax = new Vector2(1, 1);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.5f);

            // 2. 创建游戏标题文本
            gameTitleObj = CreateGameTitle(panelObj.transform, "Pixel World");

            // 3. 创建 "Start Game" 按钮
            startButtonObj = CreateButton(
                parent: panelObj.transform, 
                buttonName: "StartButton", 
                buttonText: "Start Game", 
                anchoredPosition: new Vector2(0, 50), 
                onClickAction: OnStartButtonClicked
            );

            // 4. 创建 "Quit Game" 按钮
            quitButtonObj = CreateButton(
                parent: panelObj.transform, 
                buttonName: "QuitButton", 
                buttonText: "Quit", 
                anchoredPosition: new Vector2(0, -50), 
                onClickAction: OnQuitButtonClicked
            );

            Debug.Log("Start menu UI creation completed.");
        }

        // Public method to show the UI
        public void ShowStartUI()
        {
            gameObject.SetActive(true);
            Debug.Log("Start UI is now visible.");
        }

        // 点击 "Start Game" 按钮时
        private void OnStartButtonClicked()
        {
            Debug.Log("Start Game button clicked!");
            // TODO: 在这里执行切换场景或游戏初始化逻辑
        }

        // 点击 "Quit Game" 按钮时
        private void OnQuitButtonClicked()
        {
            Debug.Log("Quit Game button clicked!");
            Application.Quit();
        }

        // Helper: 用纯脚本创建一个按钮
        private GameObject CreateButton(
            Transform parent, 
            string buttonName, 
            string buttonText, 
            Vector2 anchoredPosition, 
            UnityEngine.Events.UnityAction onClickAction){
            
            GameObject buttonObj = new GameObject(buttonName);
            buttonObj.transform.SetParent(parent, false);

            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 60);
            rectTransform.anchoredPosition = anchoredPosition;

            Image btnImage = buttonObj.AddComponent<Image>();
            btnImage.color = Color.white;

            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(onClickAction);

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = buttonText;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.black;

            textComponent.font = customFont != null ? customFont : Resources.GetBuiltinResource<Font>("Arial.ttf");
            
            int buttonFontSize = 36;
            textComponent.fontSize = buttonFontSize;

            return buttonObj;
        }

        private GameObject CreateGameTitle(Transform parent, string titleText)
        {
            GameObject titleObj = new GameObject("GameTitle");
            titleObj.transform.SetParent(parent, false);

            RectTransform rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.anchoredPosition = new Vector2(0, -30);
            rectTransform.sizeDelta = new Vector2(400, 100);

            Text textComponent = titleObj.AddComponent<Text>();
            textComponent.text = titleText;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;
            textComponent.fontSize = 72;  
            textComponent.font = (customFont != null)
                ? customFont
                : Resources.GetBuiltinResource<Font>("Arial.ttf");

            return titleObj;
        }
    }
}
