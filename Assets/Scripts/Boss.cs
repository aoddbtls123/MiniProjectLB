using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{

    [SerializeField] Collider2D bossBodyCollider;
    [SerializeField] Collider2D playerBodyCollider;

    [SerializeField] float maxHp = 100f;
    private float currentHp;

    public enum State {Idle, Pattern, PhaseTrans, Dead}
    public State currentState = State.Idle;

    [SerializeField] float phaseTransTime = 5f;

    [SerializeField] float patternDelay = 1f;
    [SerializeField] GameObject attackHitbox;
    [SerializeField] GameObject dashHitbox;
    [SerializeField] GameObject spinHitbox;
    [SerializeField] GameObject jumpHitbox;
    [SerializeField] GameObject jumpWarningCircle;

    [SerializeField] float moveSpeed = 3f;

    [SerializeField] float basicAttackWaitTime = 1f;
    [SerializeField] float basicAttackActiveTime = 1f;
    [SerializeField] float attackRange = 1f;

    [SerializeField] Vector2 dashHitboxScale = new Vector2(2f, 2f);

    [SerializeField] float dashAttackWaitTime = 1f;
    [SerializeField] float dashReadyTime = 1f;
    [SerializeField] float dashOvershoot = 2f;
    [SerializeField] float dashMoveTime = 0.08f;
  

    [SerializeField] float phase2DashCount = 3;
    [SerializeField] float dashChainDelay = 0.3f;

    [SerializeField] float spinWaitTime = 1f;
    [SerializeField] float spinActiveTime = 1f;
    [SerializeField] float spinRange = 10f;


    [SerializeField] float jumpReadyTime = 1f;
    [SerializeField] float jumpUpTime = 0.5f;
    [SerializeField] float jumpActiveTime = 0.5f;
    [SerializeField] float jumpRadius = 2f;
    [SerializeField] float jumpWarningTime = 1f;




    private Transform player;
    private Rigidbody2D rb;

    private int patternNumber = 0;
    private int currentPhase = 1;

    private bool isInvincible = false;

    private Coroutine patternLoopRoutine;
      






    void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        attackHitbox.SetActive(false);
        dashHitbox.SetActive(false);
        spinHitbox.SetActive(false);
        jumpHitbox.SetActive(false);
        jumpWarningCircle.SetActive(false);

        patternLoopRoutine = StartCoroutine(PatternLoop());



    }

    IEnumerator PatternLoop() // Boss's pattern loop
    {
        while (currentState != State.Dead)
        {
            currentState = State.Idle;
            yield return new WaitForSeconds(patternDelay);


            currentState = State.Pattern;
            if(patternNumber == 0)
            {
                yield return StartCoroutine(BasicAttack());

            }
            else if (patternNumber == 1)
            {
                yield return StartCoroutine(DashAttack());
            }
            else if (patternNumber == 2)
            {
                yield return StartCoroutine(SpinAttack());
            }
            else if (patternNumber == 3)
            {
                yield return StartCoroutine(JumpAttack());
            }
            else
            {
                yield return StartCoroutine(MultiDashAttack());
            }

                patternNumber = (patternNumber + 1) % 5;
        }
    }


    IEnumerator BasicAttack() //Boss's basic attack coroutine
    {
        while (Vector2.Distance(transform.position, player.position) > (attackRange))
        {
            Vector2 moveDir = (player.position - transform.position).normalized;

            rb.MovePosition(rb.position + moveDir* moveSpeed*Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(basicAttackWaitTime);

        Vector2 dir = (player.position - transform.position).normalized;

        transform.right = dir;

        attackHitbox.transform.position = (Vector2)transform.position + (dir * attackRange);

        attackHitbox.transform.right = dir;

        Debug.Log("보스 기본 공격");
        
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(basicAttackActiveTime);

        attackHitbox.SetActive(false);

    }


    IEnumerator DashAttack() //boss's dash attack coroutine.
    {
        Debug.Log("보스 돌진 공격 예고");

        Vector2 dashDir = (player.position - transform.position).normalized;

        transform.right = dashDir;

        yield return new WaitForSeconds(dashAttackWaitTime);

        dashDir = (player.position - transform.position).normalized;

        transform.right = dashDir;

        yield return new WaitForSeconds(dashReadyTime); 

        Debug.Log("보스 돌진 공격");

        Vector2 startDash = transform.position;

        Vector2 endDash = (Vector2)player.position + dashDir * dashOvershoot;

        dashHitbox.transform.localScale = new Vector3(dashHitboxScale.x, dashHitboxScale.y, 1f); 

        dashHitbox.transform.position = transform.position;

        dashHitbox.SetActive(true);

        Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, true);

        float elapsed = 0f;

        while (elapsed < dashMoveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dashMoveTime;
            Vector2 nextPos = Vector2.Lerp(startDash, endDash, t);
            rb.MovePosition(nextPos);
            dashHitbox.transform.position = nextPos; 
            yield return null;
        }

        rb.MovePosition(endDash);

        dashHitbox.transform.position = endDash;

        Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, false);

        dashHitbox.SetActive(false);
    }


    IEnumerator MultiDashAttack() // boss's phase 2 pattern 1 coroutine
    {
        Vector2 dashDir = (player.position - transform.position).normalized;

        transform.right = dashDir;

        yield return new WaitForSeconds(dashAttackWaitTime);

        for (int i = 0; i< phase2DashCount; i++)

        {
            dashDir = (player.position - transform.position).normalized;

            transform.right = dashDir;

            yield return new WaitForSeconds(dashAttackWaitTime);

            Vector2 startDash = transform.position;

            Vector2 endDash = (Vector2)player.position + dashDir * dashOvershoot;

            dashHitbox.transform.localScale = new Vector3(dashHitboxScale.x, dashHitboxScale.y, 1f);

            dashHitbox.transform.position = transform.position;

            dashHitbox.SetActive(true);

            Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, true);

            float elapsed = 0f;

            while (elapsed < dashMoveTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dashMoveTime;
                Vector2 nextPos = Vector2.Lerp(startDash, endDash, t);
                rb.MovePosition(nextPos);
                dashHitbox.transform.position = nextPos; 
                yield return null;
            }

            rb.MovePosition(endDash);

            dashHitbox.transform.position = endDash;

            Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, false);

            dashHitbox.SetActive(false);

            yield return new WaitForSeconds(dashChainDelay);
        }

    }



    IEnumerator SpinAttack() // boss's spin attack coroutine.
    {
        Debug.Log("보스 회전 베기 예고");


        yield return new WaitForSeconds(spinWaitTime);

        spinHitbox.transform.position = transform.position;

        spinHitbox.transform.localScale = new Vector3(spinRange * 2f, spinRange * 2f, 1f);

        spinHitbox.SetActive(true);

        Debug.Log("보스 회전 베기");

        yield return new WaitForSeconds(spinActiveTime);

        spinHitbox.SetActive(false);




    }




    IEnumerator JumpAttack() //boss's jump attack coroutine
    {
        Debug.Log("보스 점프 준비");

        yield return new WaitForSeconds(jumpReadyTime);

        Vector2 startJump = transform.position;

        Vector2 endJump = player.position;



        jumpWarningCircle.transform.position = endJump;

        jumpWarningCircle.transform.localScale = new Vector3(jumpRadius * 2f, jumpRadius * 2f, 1f);

        jumpWarningCircle.SetActive(true);

        yield return new WaitForSeconds(jumpWarningTime);

        jumpWarningCircle.SetActive(false);

        float elasped = 0f;

        while (elasped < jumpUpTime)
        {
            elasped = elasped + Time.deltaTime;
            float t = elasped / jumpUpTime;
            rb.MovePosition(Vector2.Lerp(startJump, endJump, t));
            yield return null;
        }

        rb.MovePosition(endJump);




        Debug.Log("보스 착지 공격");

        jumpHitbox.transform.position = endJump;

        jumpHitbox.transform.localScale = new Vector3(jumpRadius * 2f, jumpRadius * 2f, 1f);

        jumpHitbox.SetActive(true);

        yield return new WaitForSeconds(jumpActiveTime);

        jumpHitbox.SetActive(false);


    }


    IEnumerator PhaseTransRoutine()
    {
        currentState = State.PhaseTrans;
        isInvincible = true;

        if (patternLoopRoutine != null)
        {
            StopCoroutine(patternLoopRoutine);
        }

        attackHitbox.SetActive(false);
        dashHitbox.SetActive(false);
        spinHitbox.SetActive(false);
        jumpHitbox.SetActive(false);
        jumpWarningCircle.SetActive(false);

        Debug.Log("보스 2페이즈 전환 시작");

        yield return new WaitForSeconds(phaseTransTime);

        currentPhase = 2;

        isInvincible = false;

        currentState = State.Idle;

        Debug.Log("보스 2페이즈 전환");

        patternLoopRoutine = StartCoroutine(PatternLoop());

    }


    public void TakeDamage(float damage)
    {

        if (isInvincible)
        {
            return;
        }

        currentHp = currentHp - damage;
        Debug.Log("보스 HP: " + currentHp + "/" + maxHp);

        if (currentHp <= 0)
        {
            Die();
            return;
        }

        if (currentPhase == 1 && currentHp <= maxHp * 0.5f)
        {
            StartCoroutine(PhaseTransRoutine());
        }
    }

    private void Die()
    {
        currentState = State.Dead;

        Debug.Log("보스 토벌");

        StopAllCoroutines();

        attackHitbox.SetActive(false);

        gameObject.SetActive(false);

    }

}
