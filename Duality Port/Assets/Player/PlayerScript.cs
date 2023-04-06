using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{   
    private int moveSpeed;
    private float horizontal;
    private bool hasDoubleJump;
    private bool isGrounded;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 7;
        hasDoubleJump = true;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {   
        isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);
        if (IsGrounded() && !hasDoubleJump) {
            hasDoubleJump = true;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        animator.SetFloat("Velocity", Mathf.Abs(horizontal));
        animator.SetFloat("JumpVel", rb.velocity.y);

        if (horizontal < 0) {
            transform.localScale = new Vector3(-2, 2, 2);
        }else if (horizontal > 0) {
            transform.localScale = new Vector3(2, 2, 2);
        }

        if (Input.GetButtonUp("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, 6f);
        }else if (Input.GetButtonUp("Jump") && hasDoubleJump) {
            rb.velocity = new Vector2(rb.velocity.x, 5f);
            hasDoubleJump = false;
        }


        if(Input.GetButtonUp("YangAttack")) {
            animator.SetBool("IsYang", true);
            YangAttack();
        }
        if (Input.GetButtonUp("YinAttack")) {
            animator.SetBool("IsYang", false);
        }
    }   

    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, .1f, groundLayers);
    }

    private void YangAttack() {
        
    }
}
