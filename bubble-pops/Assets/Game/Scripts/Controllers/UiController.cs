using Game.Scripts.Ui;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private PopupTextPool popupTextPool;

        private PopupTextEntity _activePopupTextEntity;
        
        public void Initialize()
        {
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
            if (_activePopupTextEntity)
            {
                _activePopupTextEntity.StopAnimation();
                _activePopupTextEntity = null;
            }
            _activePopupTextEntity = popupTextPool.GetPopupTextFromPopTextEntity();
            _activePopupTextEntity.PlayTextAnimation(text);
        }
    }
}