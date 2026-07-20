using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    private void Awake()
    {
        Instance = this;
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
        SceneManager.LoadScene("라스트 블레이드");
    }



}
