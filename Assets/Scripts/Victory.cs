using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("lvl1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}