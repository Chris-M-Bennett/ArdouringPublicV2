using System;
using System.Collections;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player{
    public partial class PlayerOverworldControls : MonoBehaviour
    {
        
        [SerializeField] private float moveSpeed = 0.005f;
        [SerializeField] private float runSpeedDif = 0.005f;
        [SerializeField] private float maxSpeed = 0.1f;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private GameObject infoOverlay;
        [SerializeField] private Text infoText;
        [SerializeField] private Sprite interiorBG;
        [SerializeField] private Sprite exteriorBG;
        [SerializeField] private LastOpponent lastOpponent;
        
        private MoveBarsScript _transBars;
        private Vector2 _currentPosition;
        private bool _isRunning = false;
        private LayerMask _opponentMask;
        private Animator _anim;
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Running = Animator.StringToHash("Running");
        private LayerMask _structureMask;
        private Rigidbody2D _rb;
        private SpriteRenderer _sprite;

        // Start is called before the first frame update
        private void Start()
        {
            _opponentMask = LayerMask.GetMask("Opponent");
            _structureMask = LayerMask.GetMask("Structures");
            _transBars = GameObject.FindWithTag("Transition Bars").GetComponent<MoveBarsScript>();
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            if (GameManager.newGame)
            {
                transform.position = startPosition;
                infoText.text = _moveControls;
                GameManager.newGame = false;
            }
            else
            {
                _currentPosition = transform.position;
                transform.position = new Vector2(PlayerPrefs.GetFloat("playerXPos", _currentPosition.x), 
                    PlayerPrefs.GetFloat("playerYPos", _currentPosition.y));
            }
        }

        private void FixedUpdate(){
            if(Input.GetButtonDown("Run")){
                if(_isRunning){
                    moveSpeed -= runSpeedDif;
                    maxSpeed -= runSpeedDif;
                    _anim.SetBool(Running,false);
                }
                else
                {
                    moveSpeed += runSpeedDif;
                    maxSpeed += runSpeedDif;
                    _anim.SetBool(Running,true);
                }
                _isRunning = !_isRunning;
            }
            
            _rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed,Input.GetAxis("Vertical")* moveSpeed));
            float xLimit = Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed);
            float yLimit = Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed);
            _rb.velocity = new Vector2(xLimit, yLimit);
            //Moves the player on the vertical axis in the direction of the input

            //Moves the player on the horizontal axis in the direction of the input

            if (Input.GetAxis("Vertical") == 0)
            {
                _anim.SetFloat(Y,0);    
            }else
            {
                _anim.SetFloat(Y,Input.GetAxis("Vertical"));
            }

            if (Input.GetAxis("Horizontal") == 0)
            {
                _anim.SetFloat(X,0);
            }else
            {
                _anim.SetFloat(X,Input.GetAxis("Horizontal"));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            _currentPosition = transform.position;
            Collider2D opponentHit = Physics2D.OverlapCircle(_currentPosition, 0.8f, _opponentMask);

            if (opponentHit)
            {

                infoOverlay.SetActive(true);
                infoText.text = opponentHit.gameObject.GetComponent<OpponentOverworldScript>().myName;
                if (GameManager.tutorials)
                {
                    infoText.text += $"\n\n{_activateControls}";
                }
                if (Input.GetButtonDown("Activate"))
                {
                    Collider2D structureHit = Physics2D.OverlapCircle(_currentPosition, 7f, _structureMask);
                    if (structureHit)
                    {
                        GameManager.debateBG = interiorBG;
                    }else
                    {
                        GameManager.debateBG = exteriorBG;
                    }
                    
                    LastOpponent.lastOpponent = opponentHit.gameObject;
                    GameManager.debateOpponent = opponentHit.GetComponent<OpponentOverworldScript>().debatePrefab;
                    PlayerPrefs.SetFloat("playerXPos", _currentPosition.x);
                    PlayerPrefs.SetFloat("playerYPos", _currentPosition.y);
                    StartCoroutine(_transBars.MoveThoseBars(true, "Debate"));
                }
            }
            else if (GameManager.tutorials)
            {
                infoOverlay.SetActive(true);
                infoText.text = _moveControls;
            }
            else
            {
                infoOverlay.SetActive(false);
            }
        }
        
    }
}
