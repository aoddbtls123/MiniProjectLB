using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] Image hpFill;
    

    // Update is called once per frame
    void Update()
    {
        hpFill.fillAmount = boss.GetHpRatio();
    }
}
