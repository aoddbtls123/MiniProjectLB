using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float maxHp = 100f;

    private float currentHp;


    void Start()
    {
        currentHp = maxHp;
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

    void Die()
    {
        Debug.Log("보스 토벌");
        gameObject.SetActive(false);
    }

}
