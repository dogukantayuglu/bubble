using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Enums;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class MergeHandler : MonoBehaviour
    {
        [SerializeField] private float mergeDuration = 0.7f;
        [SerializeField] private IntegrityChecker integrityChecker;

        private BubbleValueSo _bubbleValueSo;
        private Action _onMergeComplete;
        private List<BubbleEntity> _bubblesToCheck;
        private List<BubbleEntity> _bubblesToMerge;

        public void Initialize(List<BubbleEntity> activeBubbles, BubbleValueSo bubbleValueSo, Action onMergeComplete)
        {
            _bubbleValueSo = bubbleValueSo;
            _onMergeComplete = onMergeComplete;
            _bubblesToCheck = new List<BubbleEntity>();
            _bubblesToMerge = new List<BubbleEntity>();
            integrityChecker.Initialize(activeBubbles);
        }

        public void CheckMerge(BubbleEntity bubbleEntity)
        {
            if (bubbleEntity.GridData == null)
            {
                _onMergeComplete.Invoke();
                return;
            }

            _bubblesToCheck.Clear();
            _bubblesToMerge.Clear();

            _bubblesToMerge.Add(bubbleEntity);
            _bubblesToCheck.Add(bubbleEntity);

            while (_bubblesToCheck.Count > 0)
            {
                GenerateMergeList();
            }

            if (_bubblesToMerge.Count < 2)
            {
                _onMergeComplete.Invoke();
                return;
            }

            var finalValue = CalculateValueAfterMerge(bubbleEntity.Value);
            MergeBubbles(finalValue);
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
                () => CheckMerge(bubbleToMerge));
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