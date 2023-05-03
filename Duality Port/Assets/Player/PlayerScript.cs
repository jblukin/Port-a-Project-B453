using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{   
    private int moveSpeed;
    private int dir;
    public float health { get; set; }
    private float charge;
    private float horizontal;
    private float slideSpeed;
    private float jumpHeight;
    private float hitRate;
    private float nextHit;
    private bool isCharging;
    private bool hasDoubleJump;
    private bool isGrounded;
    private bool isLunging;
    private bool isPunching;
    private bool facingRight;
    public bool isYang;
    public bool animEnd;
    private bool stunned;
    private AudioSource aS;

    [SerializeField] float timer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayers;
    
    [SerializeField] AudioClip[] clips;
    
    [SerializeField] Animator animator;
    [SerializeField] Animator orbAni;
    [SerializeField] GameObject orbObject;
    [SerializeField] GameObject shadow;
    [SerializeField] GameObject bar;
    [SerializeField] GameObject barHP;
    [SerializeField] GameObject textPrefab;

    private float apex = 5.45f, st = 2.275f;//Hard coded values, if position of object change, this needs to as well
    
    public float max = 0f;
    private float shadowOpac;
    

    // Start is called before the first frame update
    void Start()
    {   
        health = 100f;
        moveSpeed = 7;
        timer = 0;
        slideSpeed = 6.5f;
        jumpHeight = 8f;
        hitRate = 1f;
        nextHit = Time.time;
        hasDoubleJump = true;
        isGrounded = true;
        isCharging = false;
        isLunging = false;
        isYang = false;
        animEnd = true;
        stunned = false;
        aS = GetComponent<AudioSource>();
        aS.volume = .5f;

        barHP.SetActive(true);

    }

    // Update is called once per fram6f
    void Update()
    {   
        SetOrbPos();

        
        if (!isCharging && !isLunging && !isPunching) {
            aS.Stop();
        }
        
        dir = facingRight ? 1 : -1;

        bar.SetActive(false);
        bar.transform.GetChild(0).GetComponent<Image>().fillAmount = timer / 1;

        barHP.transform.GetChild(0).GetComponent<Image>().fillAmount = health/100f;

        shadowOpac = 1 - ((transform.position.y - st) / (apex - st));
        shadow.transform.position = new Vector3(transform.position.x, st - .6f, transform.position.z + 1);    
        shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, shadowOpac);

        isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);
        if (IsGrounded() && !hasDoubleJump) {
            hasDoubleJump = true;
        }
        if (!isCharging && !isLunging) {
            horizontal = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        animator.SetFloat("Velocity", Mathf.Abs(horizontal));
        animator.SetFloat("JumpVel", rb.velocity.y);
        animator.SetBool("IsCharging", isCharging);

        if (horizontal < 0) {
            transform.localScale = new Vector3(-2, 2, 2);
            facingRight = false;
        }else if (horizontal > 0) {
            transform.localScale = new Vector3(2, 2, 2);
            facingRight = true;
        }

        if (Input.GetButtonUp("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }else if (Input.GetButtonUp("Jump") && hasDoubleJump) {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasDoubleJump = false;
        }


        if(Input.GetButton("YangAttack")) {
            orbAni.SetBool("isYang", true);
            hitRate = .35f;
            isPunching = true;
            
            
            animator.SetBool("IsYang", true);
            isYang = true;
            animEnd = false;

            AnimatorClipInfo[] currentClip;
            currentClip = animator.GetCurrentAnimatorClipInfo(0);
            
            if(currentClip[0].clip.name == "yang_left" && !animEnd) {
                animator.StopPlayback();
                animator.SetBool("RightPunch", true);
                animator.SetBool("LeftPunch", false);
            }else {
                animator.StopPlayback();
                animator.SetBool("RightPunch", false);
                animator.SetBool("LeftPunch", true);
            }
            
            if (Time.time >= nextHit) {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, .5f, enemyLayers);
                foreach(Collider2D enemy in hitEnemies) {
                    BasicEnemyController currE = enemy.GetComponent<BasicEnemyController>();
                    float dmg = enemy.GetComponent<BasicEnemyController>().GetColorVal() ? 5f : 2f;
                    currE.TakeDamage(new AttackData(dmg, .5f, .9f));

                    SpawnText(enemy.transform.position, Mathf.Round(dmg).ToString());
                }

                nextHit = Time.time + hitRate;
            }
        }if (Input.GetButtonUp("YangAttack")) {
                aS.clip = clips[Random.Range(2, 5)];
                aS.Play();
        }
        if (Input.GetButton("YinAttack")) {
            orbAni.SetBool("isYang", false);
            hitRate = 1f;
            if(!aS.isPlaying) {
                aS.clip = clips[0];
                aS.Play();
            }
        
            bar.SetActive(true);
            animator.SetBool("IsYang", false);
            rb.velocity = new Vector2(0, 0);
            animator.SetFloat("Velocity", 0);
            animator.SetFloat("JumpVel", 0);

            if (timer < 1) {
                timer += Time.deltaTime;
                isCharging = true;
                animator.SetBool("IsCharging", true);
            }
        }

        if (Input.GetButtonUp("YinAttack") && isCharging) {
            charge = timer;
            timer = 0;
            isCharging = false;
            animator.SetBool("IsCharging", false);
            isLunging = true;
            animator.SetBool("IsLunging", true);
            
            aS.Stop();
            aS.clip = clips[1];
            aS.Play();
        }
        
        if(isLunging) {
            timer += Time.deltaTime;
            
            rb.velocity = new Vector2(slideSpeed * charge * dir, rb.velocity.y);

            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, .5f, enemyLayers);
            foreach(Collider2D enemy in hitEnemies) {
                BasicEnemyController e = enemy.GetComponent<BasicEnemyController>();
                if (e.isStunned()) {
                    break;
                }
                float dmg = e.GetColorVal() ? 3f : 7f;
                dmg *= (charge*1.5f);
                AttackData data = new AttackData(dmg, 1.5f, 2f * charge);
                e.TakeDamage(data);

                SpawnText(enemy.transform.position, Mathf.Round(dmg).ToString());
            }
            

            if (timer > charge*1.2) {
                rb.velocity = Vector2.zero;
                isLunging = false;
                timer = 0;
                charge = 0;
                animator.SetBool("IsLunging", false);
            }
        } 
    }   
    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, .1f, groundLayers);
    }

    private void TakeDamage(AttackData attackData) {
        //Debug.Log("Damage Recieved: " + attackData.damagePerHit + " - Knockback Recieved: " + attackData.knockbackDistance + " - Stun Duration Recieved: " + attackData.stunDuration);
        health -= attackData.damagePerHit;

        if (health <= 0) {
            //endgame phase
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame();
            bar.SetActive(false);
            barHP.SetActive(false);
            //Destroy(this.gameObject);
        }
        GetStunned(attackData.stunDuration);
        transform.position += new Vector3(attackData.knockbackDistance/2f, attackData.knockbackDistance/2f, 0.0f);
    }

    private void YangAnimEnd() {
        animEnd = true;
        
        animator.SetBool("RightPunch", false);
        animator.SetBool("LeftPunch", false);
        //start upercut here
        animator.SetBool("Uppercut", true);

        Collider2D[] hitE = Physics2D.OverlapCircleAll(attackPoint.position, .5f, enemyLayers);
        foreach(Collider2D enemy in hitE) {
            BasicEnemyController e = enemy.GetComponent<BasicEnemyController>();
            if (e.isStunned()) {
                break;
            }

            float dmg = e.GetColorVal() ? 5f : 3f;
            e.TakeDamage(new AttackData(dmg, 1f, 1f));

            SpawnText(e.transform.position, Mathf.Round(dmg).ToString());
        }
        isPunching = false;
    }

    private void EndUpper() {
        animator.SetBool("Uppercut", false);
    }

    private void GetStunned(float dur) {
        isLunging = false;
        isCharging = false;
        stunned = true;
        StartCoroutine(stunTimer(dur));
    }
    private IEnumerator stunTimer(float duration) 
    {
        yield return new WaitForSeconds(duration);
        stunned = false;

    }
    private void SetOrbPos() {
        Vector3 tarPos = transform.position - new Vector3(1f  * dir, 0, 0f);
        orbObject.transform.position = Vector3.Lerp(orbObject.transform.position, tarPos, Time.deltaTime * 4f);
        orbObject.transform.position = new Vector3(orbObject.transform.position.x, orbObject.transform.position.y, this.transform.position.z);
    }

    public void SpawnText(Vector3 pos, string dmg) {
        GameObject newText = Instantiate(textPrefab, pos, Quaternion.identity);
        newText.GetComponent<DamageIndicator>().SetText(dmg);
    }

    void OnEnable() {

        barHP.SetActive(true);

    }
}
