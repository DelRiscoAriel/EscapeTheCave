using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed;
    private int currentWaypoint;
    private bool tracking = true;
    Animator anim;

    private SpriteRenderer spriteRenderer;

    public bool enemy = true;

    public AudioClip enemyDeathClip;
    AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (enemy)
        {
            playerAudio = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            if (Vector2.Distance(transform.position, patrolPoints[currentWaypoint].position) > 0.2f)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentWaypoint].position, speed * Time.deltaTime);
            }
            else
            {
                currentWaypoint++;
                if (currentWaypoint >= patrolPoints.Length)
                    currentWaypoint = 0;
            }
        }
        

        Vector2 direction = (patrolPoints[currentWaypoint].position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (enemy)
        {
            if (other.gameObject.tag == "Player")
            {
                anim.SetTrigger("death");
                tracking = false;
                playerAudio.clip = enemyDeathClip;
                playerAudio.Play();
            }
        }
    }
}
