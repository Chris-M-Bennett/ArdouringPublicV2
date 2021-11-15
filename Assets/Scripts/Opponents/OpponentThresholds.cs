using System.Collections.Generic;
using UnityEngine;

namespace Opponents
{
    [CreateAssetMenu(fileName = "Opponents Emotion Thresholds", menuName = "Opponent Thresholds", order = 0)]
    public class OpponentThresholds : ScriptableObject{
        [Header("The ES thresholds at which the opponent should change emotion from lowest to highest")]public List<int> thresholds;
        [Header("The opponents emotion sprites in the following order: Happy, Sad, Angry, Afraid")]public List<Sprite> emotionSprites;
    }
}