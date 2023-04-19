using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{

    private GameObject playerReference;

    private Transform attackPoint;

    [SerializeField] private float attackRange;

    [SerializeField] private float speed;

    [SerializeField] private float rateOfAttack;

    private float attackTimer;

    [SerializeField] private float damagePerHit;

    [SerializeField] private float knockbackDistance;

    [SerializeField] private float stunDuration;

    private Animator anim;

    private bool attacking;

    private bool walking;

    private bool stunned;

    public bool colorVal; // true = black, false = white


    // Start is called before the first frame update
    void Start()
    {
        
        playerReference = GameObject.FindGameObjectWithTag("Player");

        attackPoint = transform.Find("AttackPoint");

        attackTimer = 0.0f;

        anim = transform.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        /*Temporary Code to prevent error when testing animation and movement*/
        if(playerReference == null) {

            playerReference = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("PlayerReference");

        }

        FlipDir();

        Animate();
        
        if(GetDistanceFromPlayer() <= attackRange && attackTimer < Time.time && !stunned)
            Attack();
        else if(GetDistanceFromPlayer() > attackRange && !stunned)
            Move();

    }

    private float GetDistanceFromPlayer() 
    {

        return Vector2.Distance(transform.position, playerReference.transform.position);

    }

    private void Move()
    {

        attacking = false;
        walking = true;
        stunned = false;

        transform.position = Vector2.MoveTowards(transform.position, playerReference.transform.position, speed * Time.deltaTime);

    }

    private void Attack()
    {

        attacking = true;
        walking = false;
        stunned = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1f, 1f), 0f);

        foreach (Collider2D collider in colliders) {

            if(collider.gameObject.CompareTag("Player")) {

                playerReference.SendMessage("TakeDamage", new AttackData(damagePerHit, 0.0f, 0.0f));

            }


        }

        attackTimer = Time.time + rateOfAttack;

    }

    private void FlipDir()
    {

        if(playerReference.transform.position.x - transform.position.x < 0f) {

            attackPoint.transform.localPosition = new Vector2(-0.3f, attackPoint.localPosition.y);

            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        } else {

            attackPoint.transform.localPosition = new Vector2(0.3f, attackPoint.localPosition.y);

            gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }

    }

    private void Animate() 
    {

        if(stunned) {

            anim.SetBool("stunned", true);
            anim.SetBool("attacking", false);
            anim.SetBool("walking", false);

        } else if(attacking) {

            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetBool("stunned", false);

        } else if(walking) {

            anim.SetBool("walking", true);
            anim.SetBool("attacking", false);
            anim.SetBool("stunned", false);

        }

    }

    private void TakeDamage(AttackData data) //To be used in a SendMessage when player hits enemy
    {

        // _health-=data.damagePerHit;

        // if(_health<=0.0f)
        //     Destroy(this.gameObject);

        GetStunned(data.stunDuration);
        
        transform.position-=new Vector3(data.knockbackDistance, data.knockbackDistance/2, 0.0f);

    }

    private void GetStunned(float duration)
    {

        stunned = true;
        attacking = false;
        walking = false;
        StartCoroutine(stunTimer(duration));


    }

    private IEnumerator stunTimer(float duration) 
    {

        yield return new WaitForSeconds(duration);
        stunned = false;

    }

}

public class AttackData {

    public float damagePerHit;

    public float knockbackDistance;

    public float stunDuration;

    public AttackData(float dph, float knockback, float stun) {

        damagePerHit = dph;

        knockbackDistance = knockback;

        stunDuration = stun;

    }

}
