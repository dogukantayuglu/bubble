using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class ActiveBubbleSorter : MonoBehaviour
    {
        private List<BubbleEntity> _activeBubbleEntities;
        
        public void Initialize(List<BubbleEntity> activeBubbleEntities)
        {
            _activeBubbleEntities = activeBubbleEntities;
        }

        public void Sort ()
        {
            IntArrayQuickSort (_activeBubbleEntities, 0, _activeBubbleEntities.Count - 1);
        }

        private void IntArrayQuickSort (List<BubbleEntity> data, int l, int r)
        {
            var i = l;
            var j = r;

            var x = data [(l + r) / 2].GridData.GridCoordinateValue /* find pivot item */;
            while (true) {
                while (data[i].GridData.GridCoordinateValue < x)
                    i++;
                while (x < data[j].GridData.GridCoordinateValue)
                    j--;
                if (i <= j) {
                    Exchange (data, i, j);
                    i++;
                    j--;
                }
                if (i > j)
                    break;
            }
            if (l < j)
                IntArrayQuickSort (data, l, j);
            if (i < r)
                IntArrayQuickSort (data, i, r);
        }

        private void Exchange (List<BubbleEntity> data, int m, int n)
        {
            (data [m], data [n]) = (data [n], data [m]);
        }
    }
}
