using UnityEngine;

namespace System{
    [CreateAssetMenu(fileName = "Last Opponent Tracker", menuName = "Last Opponent", order = 0)]
    public class LastOpponentTracker : ScriptableObject
    {
        private static int _lastOpponent;

        public int LastOpponent
        {
            get { return _lastOpponent;}
            set { _lastOpponent = value; }
        }
    }
}