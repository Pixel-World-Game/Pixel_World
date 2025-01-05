using UnityEngine;
using UnityEngine.UI;  // For UI.Text
using Agent;

namespace Agent
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("References")]
        public Player player;
        public Text myUIText;

        void Update()
        {
            // Option 1: Use Unity UI (e.g., a Text on a Canvas)
            // Make sure myUIText is assigned in the Inspector
            if (player == null || myUIText == null)
                return;

            // If the player has a valid world and blocktypes list
            if (player.world != null && player.world.blocktypes != null
                && player.world.blocktypes.Count > player.selectedBlockIndex)
            {
                // Example: "Selected block: Grass (Index 3)"
                string blockName = player.world.blocktypes[player.selectedBlockIndex].blockName;
                myUIText.text = "Selected block: " + blockName + " (Index " + player.selectedBlockIndex + ")";
            }
            else
            {
                // If the blocktypes are not yet loaded or out of range
                myUIText.text = "Selected block: " + player.selectedBlockIndex;
            }
        }

        private void OnGUI()
        {
            // Option 2: Use Immediate Mode GUI for a quick HUD
            if (player == null) return;

            // Build the text we want to display
            // For example: "Grass (Index 3) block selected"
            string blockName = "(No World Found)";
            if (player.world != null && player.world.blocktypes != null 
                && player.world.blocktypes.Count > player.selectedBlockIndex)
            {
                blockName = player.world.blocktypes[player.selectedBlockIndex].blockName;
            }

            string displayText = $"{blockName} (Index {player.selectedBlockIndex}) block selected";

            // Choose a style, for instance larger white text
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20; 
            style.normal.textColor = Color.white;

            // Position near the bottom left corner (10 px from left, 40 px from bottom)
            float labelWidth = 400f;
            float labelHeight = 30f;
            Rect position = new Rect(
                10f, 
                Screen.height - labelHeight - 10f, 
                labelWidth, 
                labelHeight
            );

            // Draw the label
            GUI.Label(position, displayText, style);
        }
    }
}
