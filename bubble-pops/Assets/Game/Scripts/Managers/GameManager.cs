using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridController gridController;
        [SerializeField] private BubbleController bubbleController;
        
        private void Awake()
        {
            InitializeGame();
        }

        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            bubbleController.ActivateInitThrowBubbles();
        }

        private void InitializeGame()
        {
            gridController.Initialize();
            bubbleController.Initialize(gridController);
        }
    }
}
