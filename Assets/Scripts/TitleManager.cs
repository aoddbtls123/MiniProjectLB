using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] string battleSceneName = "BattleScene";


    public void StartGame()
    {
        SceneManager.LoadScene(battleSceneName);
    }

}
