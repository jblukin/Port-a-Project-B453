using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyController : MonoBehaviour
{

    private GameObject playerReference;

    private GameObject healthBarReference;

    private GameObject canvasReference;

    private Transform attackPoint;

    [SerializeField] private float attackRange;

    [SerializeField] private float speed;

    [SerializeField] private float rateOfAttack;

    private float attackTimer;

    private float health;

    [SerializeField] private float maxHealth;

    [SerializeField] private float damagePerHit;

    [SerializeField] private float knockbackDistance;

    [SerializeField] private float stunDuration;

    private Animator anim;

    private bool attacking;

    private bool walking;

    private bool stunned;

    [SerializeField] private bool colorVal; // true = black, false = white


    // Start is called before the first frame update
    void Start()
    {
        
        playerReference = GameObject.FindGameObjectWithTag("Player");

        canvasReference = transform.Find("Canvas").gameObject;

        healthBarReference = transform.Find("Canvas/HealthBar").gameObject;

        canvasReference.SetActive(false);

        attackPoint = transform.Find("AttackPoint");

        attackTimer = 0.0f;

        anim = transform.GetComponent<Animator>();

        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        /*Temporary Code to prevent error when testing animation and movement*/
        if(playerReference == null) {

            playerReference = GameObject.FindGameObjectWithTag("Player");

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

        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, 2.4f /*Hard-coded ground Y-pos*/), 
        new Vector2(playerReference.transform.position.x, 2.4f /*Hard-coded ground Y-pos*/), speed * Time.deltaTime);

    }

    private void Attack()
    {

        attacking = true;
        walking = false;
        stunned = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1f, 1f), 0f);

        foreach (Collider2D collider in colliders) {

            if(collider.gameObject.CompareTag("Player")) {

                if(playerReference.GetComponent<PlayerScript>().isYang) //Black Player
                    if(colorVal) //Black Enemy - Less Damage Against Player
                        playerReference.SendMessage("TakeDamage", new AttackData(damagePerHit*0.75f, knockbackDistance, stunDuration));
                    else //White Enemy - Extra Damage Against Player
                        playerReference.SendMessage("TakeDamage", new AttackData(damagePerHit*1.25f, knockbackDistance, stunDuration));
                else //White Player
                    if(colorVal) //Black Enemy - Extra Damage Against Player
                        playerReference.SendMessage("TakeDamage", new AttackData(damagePerHit*1.25f, knockbackDistance, stunDuration));
                    else //White Enemy - Less Damage Against Player
                         playerReference.SendMessage("TakeDamage", new AttackData(damagePerHit*0.75f, knockbackDistance, stunDuration));

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

    public void TakeDamage(AttackData data) //To be used in a SendMessage when player hits enemy
    {

        health-=data.damagePerHit; //damage

        GameObject.Find("GameManager").GetComponent<GameManager>().totalDamageDealt += data.damagePerHit;

        if(health<=0.0f) {

            GameObject.Find("GameManager").GetComponent<GameManager>().totalEnemiesKilled++;

            Destroy(this.gameObject);

        }

        GetStunned(data.stunDuration); //stun
        
        transform.position-=new Vector3(data.knockbackDistance/2f, data.knockbackDistance/1.25f, 0.0f); //knockback

        UpdateHealthBar();

    }

    private void UpdateHealthBar() 
    {

        canvasReference.SetActive(true);

        healthBarReference.GetComponent<Image>().fillAmount = health / maxHealth;

        StartCoroutine(HealthBarDisplayTimer());

    }

    private IEnumerator HealthBarDisplayTimer() 
    {

        yield return new WaitForSeconds(0.75f);
        canvasReference.SetActive(false);

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

    public bool GetColorVal() { 
        
        return colorVal;
    
    }

    public bool isStunned() {
        return stunned;
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
