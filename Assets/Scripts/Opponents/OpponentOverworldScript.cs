using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Opponents
{
    public class OpponentOverworldScript : MonoBehaviour
    {
        [Header("Mouse over field names for description of what to add")]
        [SerializeField, Tooltip("The prefab used for this opponent in debates")] public GameObject debatePrefab;

        [SerializeField] private Animation idleAnim;
        [SerializeField] private Animation leftAnim;
        [SerializeField] private Animation rightAnim;
        [SerializeField] private Animation upAnim;
        [SerializeField] private Animation downAnim;
        [SerializeField] private Animation upLeftAnim;
        [SerializeField] private Animation upRightAnim;
        [SerializeField] private Animation downLeftAnim;
        [SerializeField] private Animation downRightAnim;
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
        void Start()
        {
            _mainRenderer = GetComponent<SpriteRenderer>();
            _mainColour = _mainRenderer.color;
            _otherRenderer = GetComponentInChildren<SpriteRenderer>();
            _otherColour = _otherRenderer.color;
            int current = 0;
            StartCoroutine(ChangeColour(current));
            _lastPosition = transform.position;
            StartCoroutine(MoveMe());
        }

        private void Update()
        {
            _currentPosition = transform.position;
            _dir = (_currentPosition - _lastPosition).normalized;

            bool isLeft, isRight, isUp, isDown = false;

            isLeft = (_dir.x > 0.5f);
            isRight = (_dir.x < -0.5f);
            isUp = (_dir.y > 0.5f);
            isDown = (_dir.y < -0.5f) ;

            if (isUp)
            {
                if (isRight)
                {

                }
                else if (isLeft)
                {

                }
                else
                {

                }
            }
            else if (isDown)
            {
                if (isRight)
                {

                }
                else if (isLeft)
                {

                }
                else
                {

                }
            }

            if (_dir.y == 1)
            {
                if (_dir.x == 1)
                {
                    //Play up right anim
                }else if (_dir.x == -1)
                {
                    //Play up left anim
                }
                else
                {
                    //Play straight up anim 
                }
            } else if (_dir.y == -1)
            {
                if (_dir.x == 1)
                {
                    //Play down right anim
                }else if (_dir.x == -1)
                {
                    //Play down left anim
                }
                else
                {
                    //Play straight anim
                }
            }
            else if (_dir.x == -1)
            {
                //Play straight left anim
            }
            else if (_dir.x == 1)
            {
                //Play straight right anim
            }
            else
            {
                //Play idle anim
            }
            //Collider2D hit = Physics2D.OverlapCircle(_currentPosition, 0.5f);
            _lastPosition = transform.position;
        }

        public void MoveMe(DirectOverworldMovementScript moveTo)
        {
            lastDest = currentDest;
            currentDest = moveTo;
        }
        public IEnumerator MoveMe()
        {

            while (true)
            {
                
                while (transform.position != currentDest.transform.position)
                {
                    var direction = (currentDest.transform.position - transform.position);
                    var dist = direction.normalized * moveSpeed;
                
                    dist = Vector3.ClampMagnitude(dist, direction.magnitude);
                
                    transform.Translate(dist);
                    yield return new WaitForSeconds(0.03f);
                }
                yield return null;
            }

            // lastDest = dest;
            // if (dest == null)
            // {
            //     yield return null;
            // }
            // while (transform.position != dest.transform.position)
            // {
            //     var direction = (dest.transform.position - transform.position);
            //     var dist = direction.normalized * moveSpeed;
            //     
            //     dist = Vector3.ClampMagnitude(dist, direction.magnitude);
            //     
            //     transform.Translate(dist);
            //     yield return new WaitForSeconds(0.03f);
            // }

        }
        

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

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                GameManager.CurrentOpponent = debatePrefab;
                SceneManager.LoadSceneAsync("Debate");
            }
        }
    }
}
