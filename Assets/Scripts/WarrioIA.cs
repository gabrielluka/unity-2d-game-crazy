using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrioIA : MonoBehaviour
{

    private GameControler gameControler;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private int movH;

    public float speed;
    public float timeWalk;
    public GameObject hitBox;
    public bool isLookLeft;


    // Start is called before the first frame update
    void Start()
    {
        gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // gameControler.player = this.transform;
         StartCoroutine("warrioWalk");
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = new Vector2(movH * speed, rigidbody2D.velocity.y);

         if(movH > 0 && isLookLeft || movH < 0 && !isLookLeft) {
            isLookLeft = !isLookLeft;
            transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
        }

        if(movH != 0) {
            animator.SetBool("isWalk", true);
        } else {
            animator.SetBool("isWalk", false);
        }
    }


     void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "HitBox") {
            gameControler.playSound(gameControler.deathSound, 0.5f);
            Destroy(col.gameObject);
            onDead();
        }
    }

    IEnumerator warrioWalk() {
        int random = Random.Range(0,100);

        if(random < 33) {
            movH = -1;
        } else if (random < 66) {
            movH = 0;
        } else {
            movH = 1;
        }

        yield return new WaitForSeconds(timeWalk);
        StartCoroutine("warrioWalk");
    }

    void onDead() {
        Destroy(this.gameObject);
    }
}
