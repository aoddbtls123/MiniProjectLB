using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCoolDown = 1f;

    [SerializeField] float attackActivetime = 1f;
    [SerializeField] float attackCoolDown = 0.5f;
    [SerializeField] GameObject attackHitbox;



    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lastLookDirection = Vector2.right;
    private BoxCollider2D playerCollider;

    private bool isDashing = false;
    private bool canDash = true;

    public bool isInvincible = false;

    private bool canAttack = true;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        attackHitbox.SetActive(false);
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;

        if (Keyboard.current.wKey.isPressed)
        {
            y = 1f;
        }
        if (Keyboard.current.sKey.isPressed)
        { 
            y = -1f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            x = -1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            x = 1f;
        }


        moveDirection = new Vector2(x, y).normalized;

        if (moveDirection != Vector2.zero)
        {
            lastLookDirection = moveDirection.normalized;
            transform.up= lastLookDirection;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !isDashing && !isAttacking && canAttack)
        {
            StartCoroutine(Attack());
        }

    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        isInvincible = true;

        playerCollider.isTrigger = true;

        rb.linearVelocity = (moveDirection * dashSpeed);
        
        yield return new WaitForSeconds(dashDuration);
        
        rb.linearVelocity = Vector2.zero;

        playerCollider.isTrigger = false;

        isDashing = false;

        isInvincible = false;

        yield return new WaitForSeconds(dashCoolDown - dashDuration);

        canDash = true;
    }


    IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackActivetime);

        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(attackCoolDown);

        canAttack = true;
        isAttacking = false;

    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

    }





}
