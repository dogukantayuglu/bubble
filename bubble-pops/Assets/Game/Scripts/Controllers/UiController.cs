using System.Collections.Generic;
using Game.Scripts.Ui;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private PopupTextPool popupTextPool;

        private List<PopupTextEntity> _activePopupTexts;

        public void Initialize()
        {
            _activePopupTexts = new List<PopupTextEntity>();
            popupTextPool.Initialize();
        }

        public void ShowMergePopupText(int mergeCount)
        {
            var text = $"{mergeCount}X";
            PlayPopupText(text);
        }

        public void ShowPerfectPopupText()
        {
            PlayPopupText("Perfect!");
        }

        private void PlayPopupText(string text)
        {
            var popupTextEntity = popupTextPool.GetPopupTextFromPopTextEntity();
            popupTextEntity.PlayTextAnimation(text);
        }
    }
}