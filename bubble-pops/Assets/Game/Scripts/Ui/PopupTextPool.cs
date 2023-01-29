using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public class PopupTextPool : MonoBehaviour
    {
        [SerializeField] private int poolCount = 5;
        [SerializeField] private Transform uiPoolParent;
        [SerializeField] private PopupTextEntity popupTextPrefab;

        private Stack<PopupTextEntity> _popupTextStack;

        public void Initialize()
        {
            PoolPopupText();
        }

        private void PoolPopupText()
        {
            _popupTextStack = new Stack<PopupTextEntity>();
            for (var i = 0; i < poolCount; i++)
            {
                var popupTextEntity = Instantiate(popupTextPrefab, Vector3.zero, Quaternion.identity, uiPoolParent);
                popupTextEntity.Initialize(ReturnToPool);
                _popupTextStack.Push(popupTextEntity);
            }
        }

        private void ReturnToPool(PopupTextEntity popupTextEntity)
        {
            _popupTextStack.Push(popupTextEntity);
        }

        public PopupTextEntity GetPopupTextFromPopTextEntity()
        {
            return _popupTextStack.Pop();
        }
    }
}
