using System;
using System.Collections;
using UnityEngine;

namespace Opponents
{
    public class OpponentOverworldScript : MonoBehaviour
    {
        [Header("Mouse over field names for description of what to add")]
        [SerializeField, Tooltip("The prefab used for this opponent in debates")] public GameObject debatePrefab;
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        [SerializeField] private Sprite upSprite;
        [SerializeField] private Sprite downSprite;
        [SerializeField] private Sprite ulTweenSprite;
        [SerializeField] private Sprite urTweenSprite;
        [SerializeField] private Sprite dlTweenSprite;
        [SerializeField] private Sprite drTweenSprite;
        private SpriteRenderer _mainRenderer;
        private Color _mainColour;
        private SpriteRenderer _otherRenderer;
        private Color _otherColour;
        private Vector2 _currentPosition;
        private Vector2 _lastPosition;
        private int xDir;
        private int yDir;
        void Start()
        {
            _mainRenderer = GetComponent<SpriteRenderer>();
            _mainColour = _mainRenderer.color;
            _otherRenderer = GetComponentInChildren<SpriteRenderer>();
            _otherColour = _otherRenderer.color;
            int current = 0;
            StartCoroutine(ChangeColour(current));
        }

        private void Update()
        {
            _currentPosition = transform.position;
            if(_currentPosition.y > _lastPosition.y)
            {
                yDir = 1;
            }else if(_currentPosition.y < _lastPosition.y)
            {
                yDir = -1;
            }else
            {
                yDir = 0;
            }
            if(_currentPosition.x > _lastPosition.x)
            {
                yDir = 1;
            }else if(_currentPosition.x < _lastPosition.x)
            {
                yDir = -1;
            }else
            {
                yDir = 0;
            }
            Collider2D hit = Physics2D.OverlapCircle(_currentPosition, 0.5f);
            _lastPosition = transform.position;
        }
        
        IEnumerator Turn(Sprite between, Sprite end){
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator ChangeColour(int current)
        {
            if (_otherColour.a == 1)
            {
                _mainColour.a = 1;
                _otherColour.a = 0;
                _mainRenderer.sprite = _otherRenderer.sprite;
                current++;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                _otherColour.a += 0.1f;
                _otherRenderer.color = _otherColour;
            }
        }
    }
}
