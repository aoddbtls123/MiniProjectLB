using UnityEngine;


[CreateAssetMenu(fileName = "BossData", menuName = "GameData/BossData")]

public class BossData : ScriptableObject
{

    public AudioClip phase1Bgm;
    public AudioClip phase2Bgm;

    //기본수치
    public float maxHp = 100f;
    public float patternDelay = 1f;
    public float phaseTransTime = 5f;
    public float phase2SpeedMultiplier = 0.7f;


    //이동
    public float moveSpeed = 32f;

    //기본 공격
    public float basicAttackWaitTime = 1f;
    public float basicAttackActiveTime = 0.2f;
    public float attackRange = 3f;


    //돌진 공격
    public Vector2 dashHitboxScale = new Vector2(1.5f, 1.5f);
    public float dashAttackWaitTime = 1f;
    public float dashReadyTime = 0.5f;
    public float dashOvershoot = 3f;
    public float dashMoveTime = 0.08f;

    //연속 돌진
    public float phase2DashCount = 3;
    public float dashChainDelay = 0.3f;

    //회전 공격
    public float spinWaitTime = 1f;
    public float spinActiveTime = 1f;
    public float spinRange = 10f;

    //점프 공격
    public float jumpReadyTime = 1f;
    public float jumpUpTime = 0.5f;
    public float jumpActiveTime = 0.5f;
    public float jumpRadius = 2f;
    public float jumpWarningTime = 1f;

    //패링 스턴시간
    public float vulnerableDuration = 2.5f;

}
