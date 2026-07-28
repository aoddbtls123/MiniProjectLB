using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHpBar : MonoBehaviour
{

    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Image hpFill;
    [SerializeField] float tweenDuration = 0.3f;

    private float lastRatio = 1f;
    private Tween hpTween;


    private void Start()
    {
        lastRatio = playerHealth.GetHpRatio();
        hpFill.fillAmount = lastRatio;
    }

    void Update()
    {
        float currentRatio = playerHealth.GetHpRatio();
        
        if (currentRatio != lastRatio)
        {
            lastRatio = currentRatio;

            hpTween?.Kill();
            hpTween = hpFill.DOFillAmount(currentRatio,tweenDuration);
        }
    }
}
