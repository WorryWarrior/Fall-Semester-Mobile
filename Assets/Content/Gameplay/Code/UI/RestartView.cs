using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Gameplay.Code.UI
{
    public class RestartView : MonoBehaviour
    {
        [SerializeField] private RectTransform self;
        [SerializeField] private TextMeshProUGUI viewText;
        [SerializeField] private Button restartButton;

        public Action RestartButtonPressed;

        public void Initialize()
        {
            restartButton.onClick.AddListener(RestartButtonPressed.Invoke);
        }

        public void Show(bool isWin, bool isServer)
        {
            self.localScale = Vector3.one;
            viewText.text = isWin ? "You won" : "You lost";
            restartButton.gameObject.SetActive(isServer);
        }

        public void Hide()
        {
            self.localScale = Vector3.zero;
        }
    }
}