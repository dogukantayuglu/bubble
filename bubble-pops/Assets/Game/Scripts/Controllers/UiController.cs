using Game.Scripts.Ui;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private PopupTextPool popupTextPool;

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
            var popupTextEntity = popupTextPool.GetPopupTextFromPopTextEntity();
            popupTextEntity.PlayTextAnimation(text);
        }
    }
}