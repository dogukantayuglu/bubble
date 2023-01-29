using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool GridDebugMode;
        
        [SerializeField] private GridController gridController;
        [SerializeField] private BubbleController bubbleController;
        [SerializeField] private UiController uiController;
        [SerializeField] private bool gridDebugMode;

        private void Awake()
        {
            GridDebugMode = gridDebugMode;
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            bubbleController.Initialize(gridController);
            gridController.Initialize(bubbleController);
            uiController.Initialize();
            SubscribeToActions();
        }

        private void SubscribeToActions()
        {
            bubbleController.OnNewMergeStarted += HandleNewMerge;
            gridController.OnAllGridCleared += HandleAllGridCleared;
        }
        
        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            bubbleController.ActivateInitThrowBubbles();
        }

        private void HandleNewMerge(int mergeCount)
        {
            uiController.ShowMergePopupText(mergeCount);
        }

        private void HandleAllGridCleared()
        {
            uiController.ShowPerfectPopupText();
        }

        private void OnDisable()
        {
            UnsubscribeToActions();
        }

        private void UnsubscribeToActions()
        {
            gridController.OnAllGridCleared -= HandleAllGridCleared;
            bubbleController.OnNewMergeStarted -= HandleNewMerge;
        }
    }
}
