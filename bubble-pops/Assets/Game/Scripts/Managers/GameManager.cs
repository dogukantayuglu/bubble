using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BubbleController bubbleController;
        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            bubbleController.Initialize();
        }
    }
}