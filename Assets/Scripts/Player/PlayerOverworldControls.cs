using System.Collections;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player{
    public partial class PlayerOverworldControls : MonoBehaviour
    {
        private Vector2 _currentPosition;
        private bool _isRunning = false;
        [SerializeField] private float moveSpeed = 0.005f;
        [SerializeField] private float runSpeedDif = 0.005f;
        [SerializeField] private LayerMask opponentMask;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private GameObject infoOverlay;
        [SerializeField] private Text infoText;
        [SerializeField] private MoveBarsScript transBars;

        // Start is called before the first frame update
        private void Start()
        {
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
            if (Input.GetAxis("Vertical") != 0)
            {//Moves the player on the vertical axis in the direction of the input
                _currentPosition.y += moveSpeed*Input.GetAxis("Vertical");
            }
            if (Input.GetAxis("Horizontal") != 0)
            {//Moves the player on the horizontal axis in the direction of the input
                _currentPosition.x += moveSpeed*Input.GetAxis("Horizontal");
            }
            if(Input.GetButtonDown("Run")){
                if(_isRunning){
                    moveSpeed -= runSpeedDif;
                }
                else{moveSpeed += runSpeedDif;}
                _isRunning = !_isRunning;
            }
            transform.position = _currentPosition;

            Collider2D hit = Physics2D.OverlapCircle(_currentPosition, 0.5f, opponentMask);
            if (hit)
            {
                infoOverlay.SetActive(true);
                infoText.text = hit.gameObject.name;
                if (GameManager.Tutorials)
                {
                    infoText.text += $"\n\n{_activateControls}";
                }
                if (Input.GetButtonDown("Activate"))
                {
                    GameManager.CurrentOpponent = hit.gameObject.GetComponent<OpponentOverworldScript>().debatePrefab;
                    PlayerPrefs.SetFloat("playerXPos", _currentPosition.x);
                    PlayerPrefs.SetFloat("playerYPos", _currentPosition.y);
                    StartCoroutine(transBars.MoveThoseBars(true, "Debate"));
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
