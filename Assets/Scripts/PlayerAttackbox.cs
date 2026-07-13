using UnityEngine;

public class PlayerAttackbox : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if(boss != null)
            {
                boss.TakeDamage(attackDamage);
            }


        }    
    }


}
