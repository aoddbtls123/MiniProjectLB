using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHp;
    private float currentHp;
    private PlayerController playerController;

    [SerializeField] float hitStopDuration = 0.05f;

    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;

    public float GetHpRatio()
    {
        return currentHp / maxHp;
    }

    void Awake()
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

        GameManager.Instance.HitStop(hitStopDuration);
        GameManager.Instance.ShakeCamera(hitStopDuration);

        GameManager.Instance.PlaySfx(hitSound);

        Debug.Log("플레이어 HP: " + currentHp + "/" + maxHp);


        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.PlaySfx(deathSound);
        Debug.Log("플레이어 사망");
        GameManager.Instance.ShowDefeat();
        gameObject.SetActive(false);
    }

}

