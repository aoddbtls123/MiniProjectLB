using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    [SerializeField] CanvasGroup blackPanel;
    [SerializeField] CanvasGroup introTextGroup;
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] CanvasGroup bossNameGroup;
    [SerializeField] CanvasGroup battleUiGroup;

    [SerializeField] string[] introSentences;
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float sentenceHoldDuration = 1f;

    [SerializeField] float textFadeDuration = 1f;
    [SerializeField] float blackFadeDuration = 1f;
    [SerializeField] float idleWaitDuration = 1.5f;
    [SerializeField] float bossNameFadeDuration = 1f;
    [SerializeField] float bossNameHoldDuration = 1.5f;

    [SerializeField] PlayerController player;
    [SerializeField] Boss boss;

    void Start()
    {
        player.SetControllable(false);

        blackPanel.alpha = 1f;
        introTextGroup.alpha = 0f;
        introText.text = "";
        bossNameGroup.alpha = 0f;
        battleUiGroup.alpha = 0f;

        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        yield return introTextGroup.DOFade(1f, textFadeDuration).WaitForCompletion();

        foreach (string sentence in introSentences)
        {
            yield return StartCoroutine(TypeSentence(sentence));
            yield return new WaitForSeconds(sentenceHoldDuration);
            introText.text = "";
        }

        yield return introTextGroup.DOFade(0f, textFadeDuration).WaitForCompletion();

        yield return blackPanel.DOFade(0f, blackFadeDuration).WaitForCompletion();

        yield return new WaitForSeconds(idleWaitDuration);

        yield return bossNameGroup.DOFade(1f, bossNameFadeDuration).WaitForCompletion();

        yield return new WaitForSeconds(bossNameHoldDuration);

        yield return bossNameGroup.DOFade(0f, bossNameFadeDuration).WaitForCompletion();

        yield return battleUiGroup.DOFade(1f, bossNameFadeDuration).WaitForCompletion();

        player.SetControllable(true);
        boss.StartBattle();
    }

    IEnumerator TypeSentence(string sentence)
    {
        introText.text = "";

        foreach (char c in sentence)
        {
            introText.text = introText.text + c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }
}