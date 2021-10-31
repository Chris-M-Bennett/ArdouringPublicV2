using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Opponents Emotion Thresholds", menuName = "Opponent Thesholds", order = 0)]
public class OpponentThresholds : ScriptableObject{
    public List<int> thresholds;
}