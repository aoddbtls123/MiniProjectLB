using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] Image hpFill;
    [SerializeField] float tweenDuration = 0.3f;

    private float lastRatio = 1f;
    private Tween hpTween;


    void Start()
    {
        lastRatio = boss.GetHpRatio();
        hpFill.fillAmount = lastRatio;
    }


    void Update()
    {

        float currentRatio = boss.GetHpRatio();

        if (currentRatio != lastRatio)
        {
            lastRatio = currentRatio;

            hpTween?.Kill();
            hpTween = hpFill.DOFillAmount(currentRatio,tweenDuration);
        }

        
    }
}
