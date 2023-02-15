using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed;    
    private float inputMovement;

    [Header("Jumping")]
    public bool isGrounded = false;
    public float defaultCoyoteTime = 0.1f;
    public float coyoteTimeRemaining = 0f;
    private bool jumpKeyHeld = false;
    private bool isOnUpwardJump = false;
    public Vector2 jumpForce = new Vector2(0, 4);
    public Vector2 counterJumpForce = new Vector2(0, -9);

    private Rigidbody2D rbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        inputMovement = Input.GetAxis("Horizontal");

        if (inputMovement < -0.1f)
            spriteRenderer.flipX = true;
        if (inputMovement > 0.1f)
            spriteRenderer.flipX = false;

        rbody.velocity = new Vector2(inputMovement * speed, rbody.velocity.y);

        if (!isGrounded)
            coyoteTimeRemaining -= Time.deltaTime;

        HandleVariableJump();

        HandleAttackInput();

        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rbody.velocity.y);
        animator.SetFloat("HorizontalVelocity", Mathf.Abs(rbody.velocity.x));
    }

    private void FixedUpdate()
    {
        // We achieve a variable jump height by applying a downward force when the player
        // ISN'T holding down the jump button.
        if (isOnUpwardJump && !jumpKeyHeld)
        {
            rbody.AddForce(counterJumpForce * rbody.mass);
        }
    }

    #region Jumping
    private void HandleVariableJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpKeyHeld = true;
            
            if (coyoteTimeRemaining > 0f)
            {
                // We apply a single jump force here that will take the player to their 
                // MAXIMUM jump height. However, if they don't continue to hold down the
                // jump key throughout their jump, a counteracting force will dampen the
                // reached height.
                isOnUpwardJump = true;
                coyoteTimeRemaining = 0f;
                rbody.AddForce(jumpForce * rbody.mass, ForceMode2D.Impulse);
            }

        } else if (Input.GetButtonUp("Jump"))
        {
            jumpKeyHeld = false;
        }
    }
    #endregion Jumping

    #region Attacking
    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            animator.SetTrigger("PlayAttack1");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            animator.SetTrigger("PlayAttack2");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            animator.SetTrigger("PlayAttack3");
        if (Input.GetKeyDown(KeyCode.Alpha4))
            animator.SetTrigger("PlayAttack4");
        if (Input.GetKeyDown(KeyCode.Alpha9))
            animator.SetTrigger("PlayHitRecoil");
        if (Input.GetKeyDown(KeyCode.Alpha0))
            animator.SetTrigger("PlayDeathAnimation");
    }
    #endregion Attacking

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;

            coyoteTimeRemaining = defaultCoyoteTime;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
