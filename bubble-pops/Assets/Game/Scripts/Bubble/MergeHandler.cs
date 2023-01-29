using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class MergeHandler : MonoBehaviour
    {
        public Action<int> OnNewMergeStarted;
        
        [SerializeField] private float mergeDuration = 0.7f;
        [SerializeField] private IntegrityChecker integrityChecker;

        private BubbleValueSo _bubbleValueSo;
        private Action _onMergeComplete;
        private List<BubbleEntity> _bubblesToCheck;
        private List<BubbleEntity> _bubblesToMerge;
        private float _explosionDuration;
        private int _mergeCount;

        public void Initialize(List<BubbleEntity> activeBubbles, BubbleValueSo bubbleValueSo, Action onMergeComplete,
            float explosionDuration)
        {
            _explosionDuration = explosionDuration;
            _bubbleValueSo = bubbleValueSo;
            _onMergeComplete = onMergeComplete;
            _bubblesToCheck = new List<BubbleEntity>();
            _bubblesToMerge = new List<BubbleEntity>();
            integrityChecker.Initialize(activeBubbles);
        }

        public void CheckMerge(BubbleEntity lastMovedBubbleEntity)
        {
            if (lastMovedBubbleEntity.GridData == null)
            {
                EndMerge();
                return;
            }

            GenerateMergeList(lastMovedBubbleEntity);

            if (_bubblesToMerge.Count < 2)
            {
                EndMerge();
                return;
            }

            CheckMergeCount();
            var finalValue = CalculateValueAfterMerge(lastMovedBubbleEntity.Value);
            MergeBubbles(finalValue);
        }

        private void GenerateMergeList(BubbleEntity lastMovedBubbleEntity)
        {
            _bubblesToCheck.Clear();
            _bubblesToMerge.Clear();

            _bubblesToMerge.Add(lastMovedBubbleEntity);
            _bubblesToCheck.Add(lastMovedBubbleEntity);

            while (_bubblesToCheck.Count > 0)
            {
                GenerateMergeList();
            }
        }

        private void CheckMergeCount()
        {
            _mergeCount++;

            if (_mergeCount > 1)
            {
                OnNewMergeStarted?.Invoke(_mergeCount);
            }
        }

        private void EndMerge()
        {
            _mergeCount = 0;
            _onMergeComplete.Invoke();
        }

        private int CalculateValueAfterMerge(int bubbleValue)
        {
            var finalValue = bubbleValue;
            for (var i = 0; i < _bubblesToMerge.Count - 1; i++)
            {
                finalValue *= 2;
            }

            finalValue = Mathf.Clamp(finalValue, 2, 2048);
            return finalValue;
        }

        private void MergeBubbles(int finalValue)
        {
            var bubbleToMerge = SelectBubbleToMerge(finalValue);
            var bubbleValueData = _bubbleValueSo.GetDataByValue(finalValue);
            bubbleToMerge.SetBubbleValue(bubbleValueData);

            var mergePosition = bubbleToMerge.GridData.Position;

            foreach (var bubbleEntity in _bubblesToMerge)
            {
                if (bubbleEntity.Equals(bubbleToMerge))
                {
                    bubbleEntity.PrepareToGetMerged(mergeDuration);
                    continue;
                }

                bubbleEntity.MoveToMergePosition(mergePosition, mergeDuration);
            }

            integrityChecker.CheckIntegrity();
            DOVirtual.DelayedCall(mergeDuration + mergeDuration * 0.1f,
                () => RestartCheckMerge(bubbleToMerge));
        }

        private void RestartCheckMerge(BubbleEntity bubbleEntity)
        {
            if (integrityChecker.ExplosionHappened())
                DOVirtual.DelayedCall(_explosionDuration, () => CheckMerge(bubbleEntity));
            
            else
                CheckMerge(bubbleEntity);
        }

        private void GenerateMergeList()
        {
            var bubbleToCheck = _bubblesToCheck[0];

            foreach (var neighbourGrid in bubbleToCheck.GridData.NeighbourGridDataList)
            {
                if (!neighbourGrid.BubbleEntity) continue;
                var neighbourBubble = neighbourGrid.BubbleEntity;

                if (_bubblesToMerge.Contains(neighbourBubble)) continue;

                if (neighbourBubble.Value == bubbleToCheck.Value)
                {
                    _bubblesToCheck.Add(neighbourBubble);
                    _bubblesToMerge.Add(neighbourBubble);
                }
            }

            _bubblesToCheck.Remove(bubbleToCheck);
        }

        private BubbleEntity SelectBubbleToMerge(int finalValue)
        {
            BubbleEntity bubbleToMerge = null;
            foreach (var bubbleEntity in _bubblesToMerge)
            {
                foreach (var bubbleNeighbourGridData in bubbleEntity.GridData.NeighbourGridDataList)
                {
                    if (!bubbleNeighbourGridData.BubbleEntity) continue;
                    if (bubbleNeighbourGridData.BubbleEntity.Value == finalValue)
                    {
                        bubbleToMerge = bubbleEntity;
                    }
                }
            }

            if (bubbleToMerge) return bubbleToMerge;

            bubbleToMerge = FindHighestBubble();

            return bubbleToMerge;
        }


        private BubbleEntity FindHighestBubble()
        {
            var highestBubble = _bubblesToMerge[0];
            var highestRow = highestBubble.GridData.Row;
            foreach (var bubbleEntity in _bubblesToMerge)
            {
                var row = bubbleEntity.GridData.Row;
                if (row >= highestRow) continue;
                highestBubble = bubbleEntity;
                highestRow = row;
            }

            return highestBubble;
        }
    }
}