using Game.Scripts.Bubble;
using Game.Scripts.Data.Bubble;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField] private BubbleGridController bubbleGridController;
        [SerializeField] private BubblePool bubblePool;
        [SerializeField] private BubbleValueSo bubbleValueSo;
        [SerializeField] private int bubbleValueRange = 5;

        public void Initialize()
        {
            bubblePool.Initialize();
            bubbleGridController.Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateBubbleAtEmptyGridPosition();
            }
        }

        private void GenerateBubbleAtEmptyGridPosition()
        {
            var gridData = bubbleGridController.GetFreeGridData();
            if (gridData == null) return;
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            
            var bubbleActivationData = new BubbleActivationData(
                gridData.GridCoordinateData, 
                gridData.Position,
                bubbleValueSo.GetSpawnableValue());
            
            bubbleEntity.Activate(bubbleActivationData);
        }

        private int RandomBubbleValue()
        {
            var randomMultiplier = Random.Range(1, bubbleValueRange + 1);
            var value = 2;
            for (var i = 0; i < randomMultiplier; i++)
            {
                value *= 2;
            }

            return value;
        }
    }
}