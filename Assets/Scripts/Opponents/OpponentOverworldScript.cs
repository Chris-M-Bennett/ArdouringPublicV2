using System.Collections;
using Player;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Opponents
{
    public class OpponentOverworldScript : MonoBehaviour
    {
        [Header("Mouse over field names for description of what to add")]
        [SerializeField, Tooltip("The prefab used for this opponent in debates")] public GameObject debatePrefab;
        [SerializeField] public DirectOverworldMovementScript lastDest;
        [SerializeField] public DirectOverworldMovementScript currentDest;
        [SerializeField] private float moveSpeed = 0.05f;


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

        void Start()
        {
            _mainRenderer = GetComponent<SpriteRenderer>();
            _mainColour = _mainRenderer.color;
            _otherRenderer = GetComponentInChildren<SpriteRenderer>();
            _otherColour = _otherRenderer.color;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _anim = GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldControls>();
            int current = 0;
            _transBars = GameObject.FindWithTag("Transition Bars").GetComponent<MoveBarsScript>();
            StartCoroutine(ChangeColour(current));
            _lastPosition = transform.position;
            _agent.destination = currentDest.transform.position;
            //StartCoroutine(MoveMe());
        }

        private void Update()
        {
            _currentPosition = transform.position;
            _dir = (_currentPosition - _lastPosition).normalized;

            //bool isLeft, isRight, isUp, isDown = false;
            _anim.SetBool(IsDown, _dir.y < -0.5f);
            _anim.SetBool(IsUp, _dir.y > 0.5f);
            _anim.SetBool(IsLeft, _dir.x < -0.5f);
            _anim.SetBool(IsRight, _dir.x > 0.5f);
            

           /* if (isDown)
            {
                
            }
            else if (isUp)
            {
                
            }
            if (isLeft)
            {
                  
            }
            else if (isRight)
            {
                 
            }
            else
            {
                _anim.Play(animationClips[8].name);  
            }*/
            
            _lastPosition = transform.position;
        }

        public void MoveMe(DirectOverworldMovementScript moveTo)
        {
            lastDest = currentDest;
            currentDest = moveTo;
            _agent.destination = moveTo.transform.position;
        }
       /* public IEnumerator MoveMe()
        {
            while (true)
            {
                while (transform.position != currentDest.transform.position)
                {
                    var direction = currentDest.transform.position - transform.position;
                    var dist = direction.normalized * moveSpeed;
                
                    dist = Vector3.ClampMagnitude(dist, direction.magnitude);
                    
                    //transform.Translate(dist);
                    yield return new WaitForSeconds(0.03f);
                }
                yield return null;
            }
        }*/
        

        IEnumerator ChangeColour(int current)
        {
            while (_otherColour.a != 1)
            {
                _otherColour.a += 0.1f;
                _otherRenderer.color = _otherColour;
                yield return new WaitForSeconds(0.1f);
            }
            _mainColour.a = 1;
            _otherColour.a = 0;
            _mainRenderer.sprite = _otherRenderer.sprite;
            current++;
            yield return new WaitForSeconds(0.5f);
        }

       /* private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject == _player.gameObject)
            {
                _player.SetDebateBG();
                GameManager.CurrentOpponent = debatePrefab;
                StartCoroutine(_transBars.MoveThoseBars(true, "Debate"));
            }
        }*/
    }
}
