using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{

    [SerializeField] Collider2D bossBodyCollider;
    [SerializeField] Collider2D playerBodyCollider;

    [SerializeField] float maxHp = 100f;
    private float currentHp;

    public enum State {Idle, Pattern, Dead}
    public State currentState = State.Idle;

    [SerializeField] float patternDelay = 1f;
    [SerializeField] GameObject attackHitbox;
    [SerializeField] GameObject dashHitbox;

    [SerializeField] float moveSpeed = 3f;

    [SerializeField] float basicAttackWaitTime = 1f;
    [SerializeField] float basicAttackActiveTime = 1f;
    [SerializeField] float attackDistance = 1f;

    [SerializeField] float dashAttackWaitTime = 1f;
    [SerializeField] float dashOvershoot = 2f;
    [SerializeField] float dashMoveTime = 0.08f;


    private Transform player;
    private Rigidbody2D rb;
    private int patternNumber = 0;






    void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        attackHitbox.SetActive(false);
        dashHitbox.SetActive(false);

        StartCoroutine(PatternLoop());

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
            else
            {
                yield return StartCoroutine(DashAttack());
            }

            patternNumber = (patternNumber + 1) % 2;
        }
    }


    IEnumerator BasicAttack() //Boss's basic attack coroutine
    {
        while (Vector2.Distance(transform.position, player.position) > (attackDistance))
        {
            Vector2 moveDir = (player.position - transform.position).normalized;

            rb.MovePosition(rb.position + moveDir* moveSpeed*Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(basicAttackWaitTime);

        Vector2 dir = (player.position - transform.position).normalized;

        transform.right = dir;

        attackHitbox.transform.position = (Vector2)transform.position + (dir * attackDistance);

        attackHitbox.transform.right = dir;

        Debug.Log("보스 기본 공격");
        
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(basicAttackActiveTime);

        attackHitbox.SetActive(false);

    }


    IEnumerator DashAttack()
    {
        Vector2 dashDir = (player.position - transform.position).normalized;
        transform.right = dashDir;

        yield return new WaitForSeconds(dashAttackWaitTime);

        Debug.Log("보스 돌진 공격");

        Vector2 startDash = transform.position;
        Vector2 endDash = (Vector2)player.position + dashDir * dashOvershoot;
        Vector2 midDash = (startDash + endDash) / 2f;
        float totalDist = Vector2.Distance(startDash, endDash);

        Vector2 dir = (endDash - startDash).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        dashHitbox.transform.rotation = Quaternion.Euler(0, 0, angle);

        dashHitbox.transform.position = midDash;
        dashHitbox.transform.localScale = new Vector3(totalDist, 1f, 1f);
        dashHitbox.SetActive(true);

        Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, true); 

        float elapsed = 0f;
        while (elapsed < dashMoveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dashMoveTime;
            rb.MovePosition(Vector2.Lerp(startDash, endDash, t));
            yield return null;
        }
        rb.MovePosition(endDash);

        Physics2D.IgnoreCollision(bossBodyCollider, playerBodyCollider, false); 

        dashHitbox.SetActive(false);
    }



    public void TakeDamage(float damage)
    {
        currentHp = currentHp - damage;
        Debug.Log("보스 HP: " +currentHp+"/"+maxHp);

        if (currentHp <= 0)
        {
            Die();
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
