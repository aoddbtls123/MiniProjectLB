using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCoolDown = 1f;

    [SerializeField] float attackActivetime = 1f;
    [SerializeField] float attackCoolDown = 0.5f;
    [SerializeField] GameObject attackHitbox;

    [SerializeField] Transform visualTransform;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Animator animator;

    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip dashSound;





    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lastLookDirection = Vector2.right;
    private BoxCollider2D playerCollider;

    private bool isDashing = false;
    private bool canDash = true;

    public bool isInvincible = false;

    private bool canAttack = true;
    private bool isAttacking = false;
    private bool canControl = true;
    


    public void SetControllable(bool value)
    {
        canControl = value;
    }
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        attackHitbox.SetActive(false);
    }

    void Update()
    {

        if (!canControl)
        {
            return;
        }

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

            transform.up = lastLookDirection;

        }

        animator.SetBool("IsMoving", moveDirection != Vector2.zero);

        visualTransform.rotation = Quaternion.identity;

        if (lastLookDirection.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (lastLookDirection.x>0f)
        {
            spriteRenderer.flipX= false;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !isDashing && !isAttacking && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }



    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        isInvincible = true;

        animator.SetTrigger("Dash");

        GameManager.Instance.PlaySfx(dashSound);

        playerCollider.isTrigger = true;

        rb.linearVelocity = (lastLookDirection * dashSpeed);
        
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

        animator.SetTrigger("Attack");

        GameManager.Instance.PlaySfx(attackSound);

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
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

    }





}
