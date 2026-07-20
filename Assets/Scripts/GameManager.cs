using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    private void Awake()
    {
        instance = this;
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);

    }

    public void ShowDefeat()
    {
        defeatPanel.SetActive(true);
    }


    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoTitle()
    {
        SceneManager.LoadScene("라스트 블레이드");
    }



}
