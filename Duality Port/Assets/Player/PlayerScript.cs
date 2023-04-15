using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{   
    private int moveSpeed;
    private float charge;
    private float horizontal;
    private float slideSpeed;
    private bool isCharging;
    private bool hasDoubleJump;
    private bool isGrounded;
    private bool isLunging;
    private bool facingRight;
    public bool isYang;

    public bool animEnd;

    [SerializeField] float timer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask enemyLayers;
    
    [SerializeField] Animator animator;
    [SerializeField] GameObject shadow;

    private float apex = 4.05f, st = 2.275f;//Hard coded values, if position of object change, this needs to as well
    
    private float shadowOpac;
    

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 7;
        timer = 0;
        slideSpeed = 6.5f;
        hasDoubleJump = true;
        isGrounded = true;
        isCharging = false;
        isLunging = false;
        isYang = false;
        animEnd = true;
    }

    // Update is called once per fram6f
    void Update()
    {   
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
            rb.velocity = new Vector2(rb.velocity.x, 6f);
        }else if (Input.GetButtonUp("Jump") && hasDoubleJump) {
            rb.velocity = new Vector2(rb.velocity.x, 5f);
            hasDoubleJump = false;
        }


        if(Input.GetButton("YangAttack")) {
            animator.SetBool("IsYang", true);
            isYang = true;
            animEnd = false;

            //check here what anim is playing,
            //if left is playing, switch to right and vice versa
            //do collides here as well
        }
        if (Input.GetButton("YinAttack")) {
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
        }
        
        if(isLunging) {
            timer += Time.deltaTime;
            int dir = facingRight ? 1 : -1;
            rb.velocity = new Vector2(slideSpeed * charge * dir, rb.velocity.y);


            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, .5f, enemyLayers);
            foreach(Collider2D enemy in hitEnemies) {
                Debug.Log("hit ene" + enemy.name);

                //HERE CHECK WHAT COLOR ENEMIES ARE AND DO CALCS FOR DAMAGE
            }

            if (timer > charge*1.2) {
                rb.velocity = Vector2.zero;
                isLunging = false;
                timer = 0;
                charge = 0;
                animator.SetBool("IsLunging", false);
            }
        }


        //pther attack
        //anim1, left punch
        //anim2, right punch
        //once anim1 is done, go to 2
        //animator event at end of 2,
        //if button pressed at any time, go back to anim1
        //if event, do uppercut

          
    }   

    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, .1f, groundLayers);
    }

    private void TakeData() {
        
    }

    private void YangAnimEnd() {
        animEnd = true;
        //start upercut here
    }
}
