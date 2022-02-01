using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletController _bullet;
    public int damage;
    public float speed;
    private float distance;
    private float targetY;

    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        speed = 1f;
        targetY = -1.8f;//-4.2f;//-1.8f; //1.32f; //0.32f; used to stop off screen, now stop immediately before player (still collide but don't stick around after being dodged)
        _bullet = GetComponent<BulletController>();

    }

    // Update is called once per frame
    void Update()
    {
        float currentY = _bullet.transform.position.y;

        distance = currentY - targetY;

        if (distance > 0f)
        {
            float direction = -1f; //targetY - currentY;
            _bullet.transform.Translate(0f, direction * speed * Time.deltaTime, 0f, Space.World);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D end)
    {

        if (end.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
   
