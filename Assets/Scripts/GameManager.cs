
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI ballsLeftValue;   // Só o número
    public TextMeshProUGUI scoreValue;       // Só o número

    public enum GameMode { Normal, Infinito };
    public GameMode currentGameMode;

    public NavMeshSurface navMeshSurface;

    private int currentBallCount = 0;
    public int pontuacaoModoNormal = 0;

    private GameObject[] spheres;
    private GameObject player;

    // ---------------------------------------------------------
    //  AWAKE — Singleton + DontDestroyOnLoad
    // ---------------------------------------------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------------------------------------------------
    //  SCENE LOADED — Recarregar UI e reiniciar nível
    // ---------------------------------------------------------
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Só reinicia se for a Scene do nível
        if (scene.name == "lvl1") 
        {
            currentGameMode = GameMode.Normal;
            RecarregarReferencias();
            ResetarNivel();
        }
        if (scene.name == "nivel_ilimitado") 
        {
            currentGameMode = GameMode.Infinito;
            RecarregarReferencias();
            ResetarNivel();
        }
    }

    // ---------------------------------------------------------
    //  RECARREGAR REFERÊNCIAS DO UI E PLAYER
    // ---------------------------------------------------------
    void RecarregarReferencias()
    {
        ballsLeftValue = GameObject.Find("BallsLeftValue")?.GetComponent<TextMeshProUGUI>();
        scoreValue = GameObject.Find("ScoreValue")?.GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // ---------------------------------------------------------
    //  RESETAR NÍVEL
    // ---------------------------------------------------------
    void ResetarNivel()
    {
        spheres = GameObject.FindGameObjectsWithTag("Spheres");
        currentBallCount = spheres.Length;

        pontuacaoModoNormal = 0;
        UpdateBallCount(currentBallCount);
        UpdateScore(pontuacaoModoNormal);

        if (currentGameMode == GameMode.Infinito)
        {
            Walls_spawn.Instance.SpawnWalls();
            esferaScript.Instance.spawnSpheres();
            //MakeMeshesReadable();
            navMeshSurface.BuildNavMesh();  
        }
    }

    // ---------------------------------------------------------
    //  UPDATE
    // ---------------------------------------------------------
    void Update()
    {
        sphereCount();
        if (currentGameMode == GameMode.Infinito){
            if (currentBallCount == 0)
            {
                esferaScript.Instance.spawnSpheres();
            }
        }
    }

    // ---------------------------------------------------------
    //  CONTAR ESFERAS
    // ---------------------------------------------------------
    void sphereCount()
    {
        spheres = GameObject.FindGameObjectsWithTag("Spheres");
        currentBallCount = spheres.Length;
        UpdateBallCount(currentBallCount);
    }

    // ---------------------------------------------------------
    //  UI — Atualizar valores
    // ---------------------------------------------------------
    public void UpdateBallCount(int count)
    {
        if (ballsLeftValue != null)
            ballsLeftValue.text = count.ToString();
    }

    public void UpdateScore(int score)
    {
        if (scoreValue != null)
            scoreValue.text = score.ToString();
    }

    // ---------------------------------------------------------
    //  ADICIONAR PONTOS
    // ---------------------------------------------------------
    public void AddScore(int amount)
    {
        pontuacaoModoNormal += amount;
        UpdateScore(pontuacaoModoNormal);
    }

    // ---------------------------------------------------------
    //  VITÓRIA
    // ---------------------------------------------------------
    public void Vitoria()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Victory");
    }
}
