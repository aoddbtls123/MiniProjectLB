using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{

    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Image hpFill;

    // Update is called once per frame
    void Update()
    {
        hpFill.fillAmount = playerHealth.GetHpRatio();
    }
}
