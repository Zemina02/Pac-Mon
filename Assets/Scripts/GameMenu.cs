using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    [SerializeField] private string gameSceneName;
    [SerializeField] private string endlessSceneName;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Endless()
    {
        SceneManager.LoadScene(endlessSceneName);
    }

    public void Quit()
    {
       Application.Quit(); 
    }
}
