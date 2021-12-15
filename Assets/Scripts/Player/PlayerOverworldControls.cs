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
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private GameObject infoOverlay;
        [SerializeField] private Text infoText;
        [SerializeField] private Sprite interiorBG;
        [SerializeField] private Sprite exteriorBG;
        
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
            if (GameManager.NewGame)
            {
                transform.position = startPosition;
                infoText.text = _moveControls;
                GameManager.NewGame = false;
            }
            else
            {
                _currentPosition = transform.position;
                transform.position = new Vector2(PlayerPrefs.GetFloat("playerXPos", _currentPosition.x), 
                    PlayerPrefs.GetFloat("playerYPos", _currentPosition.y));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            _currentPosition = transform.position;
            if (Input.GetAxis("Vertical") == 0)
            {
                _anim.SetFloat(Y,0);    
            }else
            {//Moves the player on the vertical axis in the direction of the input
                if (Mathf.Abs(_rb.velocity.y) < 0.1f)
                {
                    _rb.AddForce(new Vector2(0, moveSpeed * Input.GetAxis("Vertical")),ForceMode2D.Impulse);
                }

                _anim.SetFloat(Y,Input.GetAxis("Vertical"));  
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                _anim.SetFloat(X,0);
            }else
            {//Moves the player on the horizontal axis in the direction of the input
                if (Mathf.Abs(_rb.velocity.x) <0.1f)
                {
                    _rb.AddForce(new Vector2(moveSpeed * Input.GetAxis("Horizontal"), 0),ForceMode2D.Impulse);
                }

                _anim.SetFloat(X,Input.GetAxis("Horizontal"));
            }
            if(Input.GetButtonDown("Run")){
                if(_isRunning){
                    moveSpeed -= runSpeedDif;
                    _anim.SetBool(Running,false);
                }
                else
                {
                    moveSpeed += runSpeedDif;
                    _anim.SetBool(Running,true);
                }
                _isRunning = !_isRunning;
            }
            transform.position = _currentPosition;

            Collider2D opponentHit = Physics2D.OverlapCircle(_currentPosition, 0.8f, _opponentMask);

            if (opponentHit)
            {

                infoOverlay.SetActive(true);
                infoText.text = opponentHit.gameObject.name;
                if (GameManager.Tutorials)
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
                    
                    GameManager.CurrentOpponent = opponentHit.gameObject.GetComponent<OpponentOverworldScript>().debatePrefab;
                    PlayerPrefs.SetFloat("playerXPos", _currentPosition.x);
                    PlayerPrefs.SetFloat("playerYPos", _currentPosition.y);
                    StartCoroutine(_transBars.MoveThoseBars(true, "Debate"));
                }
            }
            else if (GameManager.Tutorials)
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
