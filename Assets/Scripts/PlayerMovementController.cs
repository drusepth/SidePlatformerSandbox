using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public float accelerationSpeed = 20f;
    public float maxSpeed = 2f;
    private Vector2 movementInput;

    [Header("Jumping")]
    public bool isGrounded = false;
    public float defaultCoyoteTime = 0.1f;
    public float coyoteTimeRemaining = 0f;
    private bool jumpKeyIsPressed = false;
    public Vector2 jumpForce = new Vector2(0, 4);
    public Vector2 counterJumpForce = new Vector2(0, -9);

    private Rigidbody2D rbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isGrounded)
            coyoteTimeRemaining -= Time.deltaTime;

        animator.SetBool("Grounded", isGrounded);

        HandleVariableJumpInput();
        // HandleAttackInput();

    }

    void FixedUpdate()
    {
        // Keyboard WASD input can include vertical input (can't figure out how to exclude it in input config),
        // so we strip any vertical input out before using it.
        Vector2 horizontalInput = new Vector2(movementInput.x, 0);

        // Horizontal movement
        // rbody.velocity = new Vector2(movementInput.x * speed, rbody.velocity.y);
        rbody.AddForce(horizontalInput * accelerationSpeed);

        // We achieve a variable jump height by applying a downward force when the player
        // ISN'T holding down the jump button.
        if (!jumpKeyIsPressed && !isGrounded)
            rbody.AddForce(counterJumpForce * rbody.mass);

        // Cap our velocity to sane levels
        rbody.velocity = new Vector2(
            Mathf.Clamp(rbody.velocity.x, -maxSpeed, maxSpeed),
            rbody.velocity.y
        );

        Debug.Log(rbody.velocity.x + " / " + accelerationSpeed);

        animator.SetFloat("VerticalVelocity", rbody.velocity.y);
        animator.SetFloat("HorizontalVelocity", Mathf.Abs(rbody.velocity.x));
    }

    void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
        
        // Handle left/right sprites based on movement direction
        if (movementInput.x < -0.1f)
            spriteRenderer.flipX = true;
        if (movementInput.x > 0.1f)
            spriteRenderer.flipX = false;
    }

    void OnJump(InputValue input)
    {
        // This fires when the player presses jump (with value 1) and when the player releases the
        // jump button (with value 0), so we simulate an isKeyPressed flag at the beginning/end
        // instead of firing on every frame in the meantime.
        jumpKeyIsPressed = (input.Get<float>() == 1f);
    }

    void OnBasicAttack(InputValue input)
    {
        /*
         * Implemented animations:
         * 
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
        */

        animator.SetTrigger("PlayAttack1");
    }

    void OnAdvancedAttack(InputValue input)
    {
        // TODO add a downward velocity to strong attacks so they look cooler from the air?
        animator.SetTrigger("PlayAttack2");
    }

    void OnInteract(InputValue input)
    {
        // TODO actual logic for interacting with objects in the world
        // (maybe use this button to show a left-DPAD option list to choose from)
        animator.SetTrigger("PlayAttack4");
    }

    void HandleVariableJumpInput()
    {
        if (jumpKeyIsPressed)
        {
            if (coyoteTimeRemaining > 0f)
            {
                // We apply a single jump force here that will take the player to their 
                // MAXIMUM jump height. However, if they don't continue to hold down the
                // jump key throughout their jump, a counteracting force will dampen the
                // reached height.
                coyoteTimeRemaining = 0f;
                rbody.AddForce(jumpForce * rbody.mass, ForceMode2D.Impulse);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                coyoteTimeRemaining = defaultCoyoteTime;
                break;
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
