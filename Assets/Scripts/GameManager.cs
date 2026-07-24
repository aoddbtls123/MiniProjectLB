using System.Linq.Expressions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    [SerializeField] Camera mainCamera;
    [SerializeField] float shakePower = 0.2f;

    private Vector3 cameraPos;

    private void Awake()
    {
        Instance = this;
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        cameraPos = mainCamera.transform.position;
    }

    //승리
    public void ShowVictory()
    {
        victoryPanel.SetActive(true);

    }

    //패배
    public void ShowDefeat()
    {
        defeatPanel.SetActive(true);
    }

    //재도전
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //타이틀로
    public void GotoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    //피격시 경직
    public void HitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }


    private IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }




}
