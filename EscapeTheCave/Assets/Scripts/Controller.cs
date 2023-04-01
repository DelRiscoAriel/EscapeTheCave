using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [Header("Physics")]
    public bool EnabledMove = true;
    public bool exiting = false;
    public float movementSpeed = 1;
    public float verticalMovementSpeed = 0.001f;
    public float jumpForce = 1;
    public float fallSpeed = -2.5f;
    bool facingRight = true;
    private Rigidbody2D rb;

    //Jump Twice
    bool inAir = false;
    bool check = false;
    RaycastHit2D isGrounded;
    public float jumpCheckOffset = 0.1f;

    Animator anim;
   
    bool onBettle = false;
    Collider2D collider;

    [Header("Damage Taken")]
    public float damageByWall = 5f;
    public float damageByEnemy = 10f;

    [Header("UI")]
    public Slider heathBar;
    public Image damageImage;
    public Image gameOverImage;
    bool damaged;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    bool healthRegen;
    public Color healthColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 5f;
    public Color loseColour = new Color(1f, 0f, 0f, 0.1f);
    public GameObject restartGameOver;
    public GameObject mainMenuGameOver;

    [Header("Win")]
    public Animator canvasWin;

    [Header("Audio")]
    public AudioClip deathClip;
    public AudioClip jumpClip;
    public AudioClip damageClip;
    public AudioClip onBettleClip;
    public AudioClip climbingClip;
    public AudioClip walkingClip;
    public AudioClip winClip;
    private AudioSource playerAudio;

    bool walkingSoundCheck = true;
    bool dead = false;
    bool climbingSoundCheck = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        if (!exiting)
        {
            if (!EnabledMove)
            {
                movement = 0;
            }
        }
        else
        {
            movement = 0;
        }
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * movementSpeed;
        if (movement < 0 || movement > 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }


        var verticalMovement = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, verticalMovement, 0) * Time.deltaTime * verticalMovementSpeed;

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        if (isGrounded)
        {
            inAir = false;
            check = false;           
        }
        else
        {
            if (check == false)
            {
                inAir = true;
                check = true;
            }
        }

        if (rb.velocity.y < fallSpeed)
        {
            anim.SetBool("onGround", false);
        }
        else
        {
            anim.SetBool("onGround", true);
        }

        //Jump
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            Jump();
        }
        //One Jump on the Air
        else if (!isGrounded && Input.GetButtonDown("Jump"))
        {
            if (inAir)
            {
                Jump();
                inAir = false;
            }
        }

        //Jump when on Top of Bettle
        if (onBettle == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        //Lose
        if (heathBar.value <= 0)
        {
            gameOverImage.color = Color.Lerp(gameOverImage.color, loseColour, 2f * Time.deltaTime);
            rb.gravityScale = 0f;
            anim.SetTrigger("death");
            dead = true;      
            exiting = true;
            collider.enabled = false;
            restartGameOver.SetActive(true);
            mainMenuGameOver.SetActive(true);
            Destroy(gameObject, 2f);
        }

        //Turn the sprite
        if (movement < 0 && facingRight)
            Flip();
        if (movement > 0 && !facingRight)
            Flip();

        //Flash Color on Damage
        if (damaged)
        {
            damageImage.color = flashColour;
            playerAudio.clip = damageClip;
            playerAudio.Play();
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        //Flash Color on Health
        if (healthRegen)
        {
            damageImage.color = healthColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        healthRegen = false;

        if (rb.velocity.y == 0)
        {
            var movements = Input.GetAxis("Horizontal");
            if (movements != 0)
            {
                playerAudio.clip = walkingClip;
                if (walkingSoundCheck)
                {
                    playerAudio.Play();
                    walkingSoundCheck = false;
                }
            }
            else
            {
                walkingSoundCheck = true;
                playerAudio.Pause();
            }
        }
        else
        {
            walkingSoundCheck = true; ;
        }

        if (dead)
        {
            playerAudio.clip = deathClip;
            playerAudio.Play();
            dead = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    void Jump()
    {
        anim.SetTrigger("jump");
        movementSpeed = 5f;
        rb.velocity = Vector2.up * jumpForce;

        playerAudio.clip = jumpClip;
        playerAudio.Play();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            anim.SetBool("idle", true);
        }
        if (other.gameObject.tag == "Hurt")
        {
            anim.SetTrigger("hurt");
            heathBar.value -= damageByWall;
            damaged = true;
        }

        if (other.gameObject.tag == "Enemy")
        {
            Jump();
            Destroy(other.gameObject, 0.5f);
            heathBar.value += 5;
        }
        if (other.gameObject.tag == "EnemyHitbox")
        {
            anim.SetTrigger("hurt");
            heathBar.value -= damageByEnemy;
            damaged = true;
        }
        if (other.gameObject.tag == "Statue")
        {
            heathBar.value = 100f;
            healthRegen = true;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hurt")
        {
            anim.SetTrigger("hurt");
            heathBar.value -= 0.5f;
            damaged = true;
        }

        if (other.gameObject.tag == "Floor")
        {
            
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            anim.SetBool("idle", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Climb")
        {
            rb.gravityScale = 0f;
            rb.drag = 2f;
            rb.velocity = new Vector2(0, 0);
            verticalMovementSpeed = 6f;
            movementSpeed = 0.000001f;
            inAir = true;
            anim.SetBool("isClimbing", true);
        }

        if (other.gameObject.tag == "Exit")
        {
            anim.SetTrigger("exit");
            exiting = true;
            restartGameOver.SetActive(true);
            mainMenuGameOver.SetActive(true);
            canvasWin.SetTrigger("win");
        }

        if (other.gameObject.tag == "Bettle")
        {
            playerAudio.clip = onBettleClip;
            playerAudio.Play();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        var verticalMovement = Input.GetAxis("Vertical");
        if (verticalMovement < 0 || verticalMovement > 0)
        {
            anim.SetBool("climbMove", true);
            playerAudio.clip = climbingClip;
            if (climbingSoundCheck)
            {
                playerAudio.Play();
                climbingSoundCheck = false;
            }         
        }
        else
        {
            anim.SetBool("climbMove", false);
            climbingSoundCheck = true;
        }
            

        if (other.gameObject.tag == "Bettle")
        {
            onBettle = true;
            var movement = Input.GetAxis("Horizontal");
            if (movement == 0)
            {
                transform.position = new Vector3(other.transform.position.x, transform.position.y/*other.transform.position.y*/, 0);
                fallSpeed = -10f;
            }
            else
            {

            }         
        }

        if (other.gameObject.tag == "Exit")
        {
            rb.gravityScale = 0f;

            EnabledMove = false;
            
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, movementSpeed * Time.deltaTime);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Climb")
        {
            rb.gravityScale = 1f;
            rb.drag = 0f;
            verticalMovementSpeed = 0.001f;
            movementSpeed = 5f;
            inAir = true;
            anim.SetBool("isClimbing", false);
        }

        if (other.gameObject.tag == "Bettle")
        {
            fallSpeed = -2.5f;
            onBettle = false;
        }
    }
}
