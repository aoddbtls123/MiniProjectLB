using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHp;
    private float currentHp;
    private PlayerController playerController;



    void Start()
    {
        currentHp = maxHp;
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(float damage)
    {
        if (playerController.isInvincible)
        {
            return;
        }

        currentHp = currentHp - damage;

        Debug.Log("플레이어 HP: " + currentHp + "/" + maxHp);


        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        GameManager.Instance.ShowDefeat();
        gameObject.SetActive(false);
    }

}

