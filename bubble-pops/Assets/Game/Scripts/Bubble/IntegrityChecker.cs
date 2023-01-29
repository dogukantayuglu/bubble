using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class IntegrityChecker : MonoBehaviour
    {
        private List<BubbleEntity> _activeBubblesEntities;
        private List<BubbleEntity> _bubblesToDrop;

        public void Initialize(List<BubbleEntity> activeBubbles)
        {
            _bubblesToDrop = new List<BubbleEntity>();
            _activeBubblesEntities = activeBubbles;
        }

        public void CheckIntegrity()
        {
            InitializeAllBubbleConnectionStatus();
            SetConnectionStatus();
            SelectUnConnectedBubbles();
        }

        private void InitializeAllBubbleConnectionStatus()
        {
            foreach (var bubbleEntity in _activeBubblesEntities)
            {
                bubbleEntity.IsConnectedToGrid = bubbleEntity.GridData.Row == 1;
            }
        }

        private void SetConnectionStatus()
        {
            for (var i = 0; i < 2; i++)
            {
                foreach (var bubbleEntity in _activeBubblesEntities)
                {
                    if (bubbleEntity.IsConnectedToGrid) continue;
                    CheckBubblesNeighbours(bubbleEntity);
                }
            }
        }

        private void CheckBubblesNeighbours(BubbleEntity bubbleEntity)
        {
            foreach (var neighbourGrid in bubbleEntity.GridData.NeighbourGridDataList)
            {
                if (!neighbourGrid.BubbleEntity) continue;
                if (neighbourGrid.BubbleEntity.IsConnectedToGrid)
                {
                    bubbleEntity.IsConnectedToGrid = true;
                    SetBubblesNeighboursConnected(bubbleEntity);
                    return;
                }
            }
        }

        private void SetBubblesNeighboursConnected(BubbleEntity bubbleEntity)
        {
            foreach (var gridData in bubbleEntity.GridData.NeighbourGridDataList)
            {
                if (!gridData.BubbleEntity) continue;
                gridData.BubbleEntity.IsConnectedToGrid = true;
            }
        }


        private void SelectUnConnectedBubbles()
        {
            foreach (var activeBubblesEntity in _activeBubblesEntities)
            {
                if (activeBubblesEntity.IsConnectedToGrid) continue;
                if (_bubblesToDrop.Contains(activeBubblesEntity)) continue;
                _bubblesToDrop.Add(activeBubblesEntity);
            }
            
            DropUnConnectedBubbles();
        }

        private void DropUnConnectedBubbles()
        {
            foreach (var bubbleEntity in _bubblesToDrop)
            {
                bubbleEntity.DropFromGrid();
            }
            
            _bubblesToDrop.Clear();
        }

        public void CheckForExplosion()
        {
            foreach (var activeBubblesEntity in _activeBubblesEntities)
            {
                if (activeBubblesEntity.Value >= 2048)
                {
                    activeBubblesEntity.Explode();
                    CheckIntegrity();
                    return;
                }
            }
        }
    }
}