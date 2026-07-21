using UnityEngine;
using System.Collections;


public class Boss : MonoBehaviour
{

    [SerializeField] Collider2D bossBodyCollider;
    [SerializeField] Collider2D playerBodyCollider;
    [SerializeField] BossData bossData;


    [SerializeField] GameObject attackHitbox;
    [SerializeField] GameObject dashHitbox;
    [SerializeField] GameObject spinHitbox;
    [SerializeField] GameObject jumpHitbox;
    [SerializeField] GameObject jumpWarningCircle;


    [SerializeField] float farDistancePoint = 5f;

    [SerializeField] float jumpFarPoints = 5f;
    [SerializeField] float dashFarPoints = 2f;
    [SerializeField] float basicFarPoints = 3f;

    [SerializeField] float basicClosePoints = 5f;
    [SerializeField] float spinClosePoints = 5f;

    

    public enum State {Idle, Pattern, PhaseTrans, Parried, Dead};
    public State currentState = State.Idle;
    public enum PatternType { Basic, Dash, Spin, Jump, MultiDash}


    private float currentHp;


    private Transform player;
    private Rigidbody2D rb;


    private int currentPhase = 1;
    private bool isInvincible = false;
    private bool isDashUnloaked = false;
    private bool hasShownHiddenPhase = false;
    private bool pendingHiddenPhase = false;
    private bool isParryable = false;
    private bool wasParried = false;

    private Coroutine patternLoopRoutine;



    private float GetDelay(float baseDelay)
    {
        return currentPhase == 2 ? baseDelay * bossData.phase2SpeedMultiplier : baseDelay;
    }

      
    private PatternType NextPatternChoose()
    {
        float distance =Vector2.Distance(transform.position, player.position);

        bool isFar = distance > farDistancePoint;

        if (isFar)
        {
            if (!isDashUnloaked)
            {
                PatternType[] patterns = { PatternType.Jump, PatternType.Basic };

                float[] weights = { jumpFarPoints, basicFarPoints };

                return PointsPick(patterns, weights);

            }
            else
            {
                PatternType dashOrMulti = currentPhase == 2 ? PatternType.MultiDash : PatternType.Dash;

                PatternType[] patterns = { PatternType.Jump, dashOrMulti, PatternType.Basic};

                float[] weights = { jumpFarPoints, dashFarPoints, basicFarPoints };

                return PointsPick(patterns, weights);
            }
        }

        else
        {
            PatternType[] patterns = { PatternType.Basic, PatternType.Spin };

            float[] weights = { basicClosePoints, spinClosePoints };

            return PointsPick(patterns, weights);

        }
    }


    private PatternType PointsPick(PatternType[] patterns, float[] weights)
    {
        float total = 0f;
        foreach (float w in weights)
        {
            total = total + w;
        }

        float rand = Random.Range(0f, total);
        float sum = 0f;

        for (int i = 0; i < patterns.Length; i++)
        {
            sum = sum+ weights[i];

            if (rand <= sum)
            {
                return patterns[i];
            }

        }


        return patterns[patterns.Length - 1];
    }







    void Start()
    {
        currentHp = bossData.maxHp;
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
            yield return new WaitForSeconds(GetDelay(bossData.patternDelay));


            currentState = State.Pattern;

            if (pendingHiddenPhase)
            {
                pendingHiddenPhase = false;

                yield return StartCoroutine(HiddenPhaseRoutine());

                continue;
            }

            PatternType nextPattern = NextPatternChoose();



            switch (nextPattern)

            {
                case PatternType.Basic:
                    yield return StartCoroutine(BasicAttack());
                    break;

                case PatternType.Dash:
                    yield return StartCoroutine(DashAttack());
                    break;

                case PatternType.Spin:
                    yield return StartCoroutine(SpinAttack());
                    break;

                case PatternType.Jump:
                    yield return StartCoroutine(JumpAttack());
                    break;

                case PatternType.MultiDash:
                    yield return StartCoroutine(MultiDashAttack());
                    break;


            }
        }
    }


    IEnumerator BasicAttack() //Boss's basic attack coroutine
    {
        while (Vector2.Distance(transform.position, player.position) > (bossData.attackRange))
        {
            Vector2 moveDir = (player.position - transform.position).normalized;

            rb.MovePosition(rb.position + moveDir* bossData.moveSpeed*Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(GetDelay(bossData.basicAttackWaitTime));

        Vector2 dir = (player.position - transform.position).normalized;

        transform.right = dir;

        attackHitbox.transform.position = (Vector2)transform.position + (dir * bossData.attackRange);

        attackHitbox.transform.right = dir;

        Debug.Log("보스 기본 공격");
        
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(GetDelay(bossData.basicAttackActiveTime));

        attackHitbox.SetActive(false);

    }


    IEnumerator DashAttack() //boss's dash attack coroutine.
    {
        Debug.Log("보스 돌진 공격 예고");

        Vector2 dashDir = (player.position - transform.position).normalized;

        transform.right = dashDir;

        yield return new WaitForSeconds(GetDelay(bossData.dashAttackWaitTime));

        dashDir = (player.position - transform.position).normalized;

        transform.right = dashDir;

        Debug.Log("패리 가능 구간");

        isParryable = true;

        float readyElapsed = 0f;

        while (readyElapsed < GetDelay(bossData.dashReadyTime))
        {
            if (wasParried)
            {

            }
        }



        yield return new WaitForSeconds(GetDelay(bossData.dashReadyTime)); 

        Debug.Log("보스 돌진 공격");

        Vector2 startDash = transform.position;

        Vector2 endDash = (Vector2)player.position + dashDir * bossData.dashOvershoot;

        dashHitbox.transform.localScale = new Vector3(bossData.dashHitboxScale.x, bossData.dashHitboxScale.y, 1f); 

        dashHitbox.transform.position = transform.position;

        dashHitbox.SetActive(true);

        Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, true);

        float elapsed = 0f;

        while (elapsed < bossData.dashMoveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bossData.dashMoveTime;
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

        yield return new WaitForSeconds(GetDelay(bossData.dashAttackWaitTime));

        for (int i = 0; i< bossData.phase2DashCount; i++)

        {
            dashDir = (player.position - transform.position).normalized;

            transform.right = dashDir;

            yield return new WaitForSeconds(GetDelay(bossData.dashAttackWaitTime));

            Vector2 startDash = transform.position;

            Vector2 endDash = (Vector2)player.position + dashDir * bossData.dashOvershoot;

            dashHitbox.transform.localScale = new Vector3(bossData.dashHitboxScale.x, bossData.dashHitboxScale.y, 1f);

            dashHitbox.transform.position = transform.position;

            dashHitbox.SetActive(true);

            Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, true);

            float elapsed = 0f;

            while (elapsed < bossData.dashMoveTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / bossData.dashMoveTime;
                Vector2 nextPos = Vector2.Lerp(startDash, endDash, t);
                rb.MovePosition(nextPos);
                dashHitbox.transform.position = nextPos; 
                yield return null;
            }

            rb.MovePosition(endDash);

            dashHitbox.transform.position = endDash;

            Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, false);

            dashHitbox.SetActive(false);

            yield return new WaitForSeconds(GetDelay(bossData.dashChainDelay));
        }

    }



    IEnumerator SpinAttack() // boss's spin attack coroutine.
    {
        Debug.Log("보스 회전 베기 예고");


        yield return new WaitForSeconds(GetDelay(bossData.spinWaitTime));

        spinHitbox.transform.position = transform.position;

        spinHitbox.transform.localScale = new Vector3(bossData.spinRange * 2f, bossData.spinRange * 2f, 1f);

        spinHitbox.SetActive(true);

        Debug.Log("보스 회전 베기");

        yield return new WaitForSeconds(GetDelay(bossData.spinActiveTime));

        spinHitbox.SetActive(false);




    }




    IEnumerator JumpAttack() //boss's jump attack coroutine
    {
        Debug.Log("보스 점프 준비");

        yield return new WaitForSeconds(GetDelay(bossData.jumpReadyTime));

        Vector2 startJump = transform.position;

        Vector2 endJump = player.position;



        jumpWarningCircle.transform.position = endJump;

        jumpWarningCircle.transform.localScale = new Vector3(bossData.jumpRadius * 2f, bossData.jumpRadius * 2f, 1f);

        jumpWarningCircle.SetActive(true);

        yield return new WaitForSeconds(GetDelay(bossData.jumpWarningTime));

        jumpWarningCircle.SetActive(false);

        float elasped = 0f;

        while (elasped < bossData.jumpUpTime)
        {
            elasped = elasped + Time.deltaTime;
            float t = elasped / bossData.jumpUpTime;
            rb.MovePosition(Vector2.Lerp(startJump, endJump, t));
            yield return null;
        }

        rb.MovePosition(endJump);




        Debug.Log("보스 착지 공격");

        jumpHitbox.transform.position = endJump;

        jumpHitbox.transform.localScale = new Vector3(bossData.jumpRadius * 2f, bossData.jumpRadius * 2f, 1f);

        jumpHitbox.SetActive(true);

        yield return new WaitForSeconds(GetDelay(bossData.jumpActiveTime));

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

        yield return new WaitForSeconds(GetDelay(bossData.phaseTransTime));

        currentPhase = 2;

        isInvincible = false;

        currentState = State.Idle;

        Debug.Log("보스 2페이즈 전환");

        patternLoopRoutine = StartCoroutine(PatternLoop());

    }


    IEnumerator DeathCoroutine()
    {

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);

        GameManager.Instance.ShowVictory();

    }



    IEnumerator HiddenPhaseRoutine()
    {
        Debug.Log("숨은 페이즈 분기점 발동");

        yield return StartCoroutine(SpinAttack());
        yield return StartCoroutine(DashAttack());

        isDashUnloaked = true;
    }



    public void TakeDamage(float damage)
    {

        if (isInvincible)
        {
            return;
        }

        currentHp = currentHp - damage;
        Debug.Log("보스 HP: " + currentHp + "/" + bossData.maxHp);

        if (currentHp <= 0)
        {
            Die();
            return;
        }

        if (!hasShownHiddenPhase && currentPhase == 1 && currentHp <= bossData.maxHp * 0.8f)
        {
            hasShownHiddenPhase = true;
            pendingHiddenPhase = true;
        }

        if (currentPhase == 1&& currentHp <= bossData.maxHp *0.5f)
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
        
        StartCoroutine(DeathCoroutine());

    }



}
