using UnityEngine;

public class PlayerAttackbox : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;
    [SerializeField] GameObject hitEffectPrefab;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if(boss != null)
            {
                boss.TakeDamage(attackDamage);
                Instantiate (hitEffectPrefab,transform.position,Quaternion.identity);
            }


        }    
    }


}
