using System.Collections.Generic;
using UnityEngine;

namespace Opponents
{
    [CreateAssetMenu(fileName = "Opponents Emotion Thresholds", menuName = "Opponent Thresholds", order = 0)]
    public class OpponentThresholds : ScriptableObject{
        public List<int> thresholds;
        public List<Sprite> emotionSprites;
    }
}