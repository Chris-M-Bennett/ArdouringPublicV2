using UnityEngine;

namespace System{
    [CreateAssetMenu(fileName = "Last Opponent Tracker", menuName = "Last Opponent", order = 0)]
    public class LastOpponent : ScriptableObject{
        public static GameObject lastOpponent = null;
    }
}