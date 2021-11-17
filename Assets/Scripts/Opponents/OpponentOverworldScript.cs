using System.Collections;
using UnityEngine;

namespace Opponents
{
    public class OpponentOverworldScript : MonoBehaviour
    {
        [Header("Mouse over field names for description of what to add")]
        [SerializeField, Tooltip("Thresholds Scriptable Object '{}' for this opponent")]private OpponentThresholds myThresholds;
        [SerializeField, Tooltip("The prefab used for this opponent in debates")] public GameObject debatePrefab;
        private SpriteRenderer _mainRenderer;
        private Color _mainColour;
        private SpriteRenderer _otherRenderer;
        private Color _otherColour;
        void Start()
        {
            _mainRenderer = GetComponent<SpriteRenderer>();
            _mainColour = _mainRenderer.color;
            _otherRenderer = GetComponentInChildren<SpriteRenderer>();
            _otherColour = _otherRenderer.color;
            int current = 0;
            StartCoroutine(ChangeColour(current));
        }

        IEnumerator ChangeColour(int current)
        {
            if (_otherColour.a == 1)
            {
                _otherColour.a = 0;
                _mainRenderer.sprite = _otherRenderer.sprite;
                current++;
                if (current == myThresholds.emotionSprites.Count - 1)
                {
                    _otherRenderer.sprite = myThresholds.emotionSprites[0];
                    current = 0;
                }
                else
                {
                    _otherRenderer.sprite = myThresholds.emotionSprites[current + 1];
                }
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                _otherColour.a += 0.1f;
            }
        }
    }
}
