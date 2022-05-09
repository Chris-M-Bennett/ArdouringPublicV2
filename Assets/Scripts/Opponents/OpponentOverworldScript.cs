using System;
using Player;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Opponents
{
    public class OpponentOverworldScript : MonoBehaviour
    {
        [Header("Mouse over field names for description of what to add")]
        [SerializeField, Tooltip("The opponent's name")] public string myName;
        [SerializeField, Tooltip("The prefab used for this opponent in debates")]
        public GameObject debatePrefab;
        [SerializeField, Tooltip("The prefab used if this opponent is defeated")]
        private GameObject defeatedPrefab;
        [SerializeField, Tooltip("The speed at which the opponent should move between points")] private float moveSpeed = 2f;

        private SpriteRenderer _mainRenderer;
        private Color _mainColour;
        private SpriteRenderer _otherRenderer;
        private Color _otherColour;
        private Vector2 _currentPosition;
        private Vector2 _lastPosition;
        private Vector2 _dir;
        private Animator _anim;
        private MoveBarsScript _transBars;
        private NavMeshAgent _agent;
        private PlayerOverworldControls _player;
        private static readonly int IsLeft = Animator.StringToHash("IsLeft");
        private static readonly int IsDown = Animator.StringToHash("IsDown");
        private static readonly int IsUp = Animator.StringToHash("IsUp");
        private static readonly int IsRight = Animator.StringToHash("IsRight");
        
        private DirectOverworldMovementScript _lastDest;
        public DirectOverworldMovementScript LastDest
        {
            get { return _lastDest; }
            set { _lastDest = value; }
        } 
        private DirectOverworldMovementScript currentDest;
        public DirectOverworldMovementScript CurrentDest
        {
            get { return currentDest; }
            set { currentDest = value; }
        } 

        void Start()
        {
            _mainRenderer = GetComponent<SpriteRenderer>();
            _mainColour = _mainRenderer.color;
            _otherRenderer = GetComponentInChildren<SpriteRenderer>();
            _otherColour = _otherRenderer.color;
            _anim = GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldControls>();
            transform.eulerAngles = Vector3.zero;
            if(GetComponent<NavMeshAgent>()){
                _agent = GetComponent<NavMeshAgent>();
                _agent.updateRotation = false;
                _agent.updateUpAxis = false;
                _lastPosition = transform.position;
                _agent.speed = moveSpeed;
                _agent.destination = currentDest.transform.position; 
            }
        }

        private void Update()
        {
            _currentPosition = transform.position;
            _dir = (_currentPosition - _lastPosition).normalized;
            
            _anim.SetBool(IsDown, _dir.y < -0.5f);
            _anim.SetBool(IsUp, _dir.y > 0.5f);
            _anim.SetBool(IsLeft, _dir.x < -0.5f);
            _anim.SetBool(IsRight, _dir.x > 0.5f);

            _lastPosition = transform.position;
        }
        

        public void MoveMe(DirectOverworldMovementScript moveTo)
        {
            _lastDest = currentDest;
            currentDest = moveTo;
            _agent.destination = moveTo.transform.position;
        }
    }
}
