using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public bool isLookLeft;
    public Transform groundCheck;
    public GameObject hitBoxPreFab;
    public Transform hand;

    private bool isGrounded;
    private Animator playerAnimator;
    private Rigidbody2D body;
    private bool isAttack;
    private GameControler gameControler;
    private SpriteRenderer playerSprite;

    public string nextScene;
    public Color hitColor;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
        playerSprite =  GetComponent<SpriteRenderer>();
        gameControler.player = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
       HandlerMove();
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Collect") {
            gameControler.playSound(gameControler.coinSound, 0.2f);
            gameControler.getCoin();
            Destroy(col.gameObject);
        } else if(col.gameObject.tag == "Damage") {

           if (gameControler.life > 0) {
                gameControler.getHit();
                StartCoroutine("damagerControler");
            }
        } else if(col.gameObject.tag == "Flag") {
            if(gameControler.coindCount >= 0) {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        } else if(col.gameObject.tag == "abismo") {
            gameControler.life = 0;
            gameControler.getHit();
            StartCoroutine("damagerControler");
        }
    }


    // My Functions
    void HandlerMove() {
        Jump();
        Attack();
        Movements();
    }

    void Movements() {
        float mov = Input.GetAxis("Horizontal");

        if(mov > 0 && isLookLeft || mov < 0 && !isLookLeft) {
            isLookLeft = !isLookLeft;
            transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
        } 

        if(isAttack && isGrounded) {
            mov = 0;
        }

        float speedY = body.velocity.y;
        body.velocity = new Vector2(mov * Speed, speedY);
        SetAnimation(mov, speedY);
    }

    void Jump() {
        if(Input.GetButtonDown("Jump") && isGrounded) {
            gameControler.playSound(gameControler.jumpSound, 0.15f);
            body.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
    }

    void Attack() {
         if(Input.GetButtonDown("Fire1")) {
            gameControler.playSound(gameControler.attackSound, 0.15f);
            isAttack = true;
            playerAnimator.SetTrigger("attack");
        }
    }

    void EndAttack() {
        isAttack = false;
    }

    void SetAnimation(float mov, float speedy) {
        playerAnimator.SetInteger("h", (int) mov);
        playerAnimator.SetBool("isGroundCheck", isGrounded);
        playerAnimator.SetFloat("speedY", speedy);
        playerAnimator.SetBool("isAttack", isAttack);
    }

    void hitAttack() {
        GameObject hitBoxTemp = Instantiate(hitBoxPreFab, hand.position, transform.localRotation); 
        Destroy(hitBoxTemp, 0.2f);
    }

    void footStep() {
        if(Random.Range(0, 10) < 5) {
            gameControler.playSound(gameControler.stepSoundA, 0.15f);
        } else {
            gameControler.playSound(gameControler.stepSoundB, 0.15f);
        }
    }

    IEnumerator damagerControler() {
        gameControler.playSound(gameControler.damageSound, 0.4f);
        
        playerSprite.color = hitColor;
        for(int i = 0; i < 5 ; i ++) {
                playerSprite.enabled = false;
                yield return new WaitForSeconds(0.2f);
                playerSprite.enabled = true;
                yield return new WaitForSeconds(0.2f);
        }
        playerSprite.color = Color.white;
    }
}
