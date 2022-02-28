using System.Collections.Generic;
using UnityEngine;

namespace Opponents
{
    [CreateAssetMenu(fileName = "Opponents Emotion Thresholds", menuName = "Opponent Thresholds", order = 0)]
    public class OpponentThresholds : ScriptableObject
    {
        [Header("The ES thresholds at which the opponent should change emotion from lowest to highest")]public List<int> thresholds;
        [Header("The emotions this opponent can experience in the following order: Happy = 0, Sad = 1, Angry = 2, Confident = 3, Afraid = 4")]public List<int> emotions;
    }
}