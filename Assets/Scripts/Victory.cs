using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class Victory : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReplayLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(GameManager.Instance.currentGameMode == GameMode.Normal)
        {
        SceneManager.LoadScene("lvl1");
        }
        else if (GameManager.Instance.currentGameMode == GameMode.Infinito)
        {
            SceneManager.LoadScene("nivel_ilimitado");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}