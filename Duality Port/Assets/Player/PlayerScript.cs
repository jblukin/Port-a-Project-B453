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

    [SerializeField] float timer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] Animator animator;
    [SerializeField] Sprite kickSprite;
    

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
    }

    // Update is called once per frame
    void Update()
    {   
        isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);
        if (IsGrounded() && !hasDoubleJump) {
            hasDoubleJump = true;
        }
        if (!isCharging) {
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
        //is lunging
        
        if(isLunging) {
            timer += Time.deltaTime;
            int dir = facingRight ? 1 : -1;
            rb.velocity = new Vector2(slideSpeed * charge * dir, rb.velocity.y);
            if (timer > 1) {
                rb.velocity = Vector2.zero;
                isLunging = false;
                timer = 0;
                charge = 0;
                animator.SetBool("IsLunging", false);
            }
        }


        //pther attack
        //GetCurrentAnimatorClipInfo
        //If double punch anim is playhing && button is pressed
        //do double punch again
    }   

    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, .1f, groundLayers);
    }


    private void YinAttack() {
        //find some way to smoothly move across the floor
        Debug.Log("hori"+ horizontal);
        rb.velocity = new Vector2(10f * moveSpeed, 0f);
    }

    private void TakeData() {
        
    }
}
