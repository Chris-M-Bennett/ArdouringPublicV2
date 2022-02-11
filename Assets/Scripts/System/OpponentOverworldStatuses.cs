using System.Collections.Generic;
using UnityEngine;

namespace System{
    [CreateAssetMenu(fileName = "AreaStatuses", menuName = "Opponent Statuses", order = 0)]
    public class OpponentOverworldStatuses : ScriptableObject{
        public int[] statuses;

        private void OnValidate(){
            for (int i = 0; i < statuses.Length; i++)
            {
                if (!(statuses[i] >= -1 && statuses[i] <= 1))
                {
                    Debug.LogError("Invalid status! Must be -1, 0 or 1");
                    statuses[i] = 0;
                }
            }
        }
    }
}