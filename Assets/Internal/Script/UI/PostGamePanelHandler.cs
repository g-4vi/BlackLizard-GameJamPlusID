using TMPro;
using UnityEngine;

namespace GameJamPlus {
    public class PostGamePanelHandler : MonoBehaviour {

        [Header("UI References")]
        public TMP_Text finalScoreText;

        void OnEnable() {
            if (PlayerManager.Instance != null) {
                int finalScore = PlayerManager.Instance.GetMana();
                finalScoreText.text = finalScore.ToString();
            }
        }

    }
}