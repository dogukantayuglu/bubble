using System.Collections.Generic;
using Game.Scripts.Data.Bubble;
using UnityEngine;

namespace Game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Bubble/Bubble Value")]
    public class BubbleValueSo : ScriptableObject
    {
        [SerializeField] private List<BubbleValueData> bubbleValueDataList;

        public BubbleValueData GetSpawnableValue()
        {
            return bubbleValueDataList[Random.Range(0, 3)];
        }

        public Color GetColorByValue(int value)
        {
            var bubbleValueData = GetDataByValue(value);
            return bubbleValueData?.color ?? Color.black;
        }

        public BubbleValueData GetDataByValue(int value)
        {
            foreach (var bubbleValueData in bubbleValueDataList)
            {
                if (value.Equals(bubbleValueData.value))
                    return bubbleValueData;
            }

            return null;
        }
    }
}