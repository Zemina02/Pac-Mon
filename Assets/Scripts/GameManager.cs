
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
    public int pontuacaoModoInfinito = 0;
    private float infiniteTimeAccumulator = 0f;

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
        if (scene.name == "lvl1") // <-- ALTERA PARA O NOME DA TUA SCENE
        {
            RecarregarReferencias();
            ResetarNivel();
        }
        if (scene.name == "nivel_ilimitado") // <-- ALTERA PARA O NOME DA TUA SCENE
        {
            // Se quiser resetar o jogo ao voltar para a vitória, pode chamar ResetarNivel() aqui também
            Walls_spawn.Instance.SpawnWalls();
                esferaScript.Instance.spawnSpheres();
        }
    }

    // ---------------------------------------------------------
    //  START — Apenas usado na primeira vez
    // ---------------------------------------------------------
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        pontuacaoModoInfinito = 0;
        infiniteTimeAccumulator = 0f;

        UpdateBallCount(currentBallCount);
        UpdateScore(pontuacaoModoInfinito);

        if (currentGameMode == GameMode.Infinito)
        {
            Walls_spawn.Instance.SpawnWalls();
            esferaScript.Instance.spawnSpheres();
            navMeshSurface.BuildNavMesh();
        }
    }

    // ---------------------------------------------------------
    //  UPDATE — Lógica do modo infinito
    // ---------------------------------------------------------
    void Update()
    {
        if (currentGameMode == GameMode.Infinito)
        {
            sphereCount();
            HandleInfiniteTime();
        }
        else
        {
            sphereCount();
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
        pontuacaoModoInfinito += amount;
        UpdateScore(pontuacaoModoInfinito);
    }

    // ---------------------------------------------------------
    //  PONTUAÇÃO POR TEMPO (modo infinito)
    // ---------------------------------------------------------
    private void HandleInfiniteTime()
    {
        infiniteTimeAccumulator += Time.deltaTime;

        if (infiniteTimeAccumulator >= 1f)
        {
            int seconds = Mathf.FloorToInt(infiniteTimeAccumulator);
            pontuacaoModoInfinito += seconds;
            infiniteTimeAccumulator -= seconds;

            UpdateScore(pontuacaoModoInfinito);
        }
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
