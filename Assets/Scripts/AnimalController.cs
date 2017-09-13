using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    public float moveSpeed = 5;
    public float jumpHeight = 5;
    public bool doubleJumpEnable = true;

    private float horizontalInput;
    private float verticalInput;

    public Transform wallCheck;
    private bool atWall;

    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask whatIsGround;
    public bool isGrounded = true;
    private bool hasDoubleJumped = false;

    private bool isDashing = false;
    private bool canDash = true;
    public float dashCooldown = 2;
    public float dashChargeTime = 0.5f;
    public float dashPower = 10;
    public float dashChargeHorizontalDrag = 10;
    public float dashChargeVerticalDrag = 10;
    public float dashDuration = 0.3f;
    public float afterDashMovePenalty = 5;

    private Rigidbody2D rb;
    private Animator anim;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        atWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, whatIsGround);

    }

    void Update () {
        if (!FindObjectOfType<Game>().isGameOver) {
            userInput();
        }
        updateAnimations();
	}

    private void userInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (horizontalInput < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (!isDashing) {

            if (Input.GetKeyDown(KeyCode.Return) && canDash) {
                rb.velocity = new Vector2(rb.velocity.x/dashChargeHorizontalDrag, rb.velocity.y/dashChargeVerticalDrag);
                isDashing = true;
                canDash = false;
                StartCoroutine(dash());
                return;
            }

            if (!atWall) {
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded && !hasDoubleJumped && doubleJumpEnable) {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                hasDoubleJumped = true;
                
            }

            if (isGrounded) {
                hasDoubleJumped = false;
            }
        }
    }

    void updateAnimations() {
        if(Mathf.Abs(rb.velocity.x) > 0) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        if (isGrounded) {
            anim.SetBool("isGrounded", true);
        } else {
            anim.SetBool("isGrounded", false);
        }
        anim.SetFloat("Blend", rb.velocity.y);
    }

    private IEnumerator dash() {
        anim.SetTrigger("Dash");
        yield return new WaitForSeconds(dashChargeTime);

        Vector2 dashDirection = new Vector2(horizontalInput * dashPower, verticalInput * dashPower);
        if (verticalInput < 0) {
            dashDirection.y = 0;
        }

        rb.velocity = dashDirection;

        yield return new WaitForSeconds(dashDuration);
        rb.velocity = new Vector2(rb.velocity.x - afterDashMovePenalty, rb.velocity.y- afterDashMovePenalty);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
