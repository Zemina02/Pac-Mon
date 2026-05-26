using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public enum GameMode { Normal, Infinito };
    public GameMode currentGameMode;
    public TextMeshProUGUI ballCountText;
    public TextMeshProUGUI scoreText;
    public int pontuacaoModoInfinito;
    private float infiniteTimeAccumulator = 0f;


    GameObject[] spheres;
    GameObject player;
    private int currentBallCount = 0;
    public static GameManager Instance;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (currentGameMode == GameMode.Infinito)
        {
            pontuacaoModoInfinito = 0;
            Walls_spawn.Instance.SpawnWalls();
            esferaScript.Instance.spawnSpheres();
            UpdateScoreText();
        }
    }

    void sphereCount()
    {
        spheres = GameObject.FindGameObjectsWithTag("Spheres");
        int count = (spheres != null) ? spheres.Length : 0;
        currentBallCount = count;
        Debug.Log("Number of spheres: " + count);
        UpdateBallCountText(count);
    }

    void openDoor()
    {
        int sphereCount = spheres.Length;

        if (sphereCount == 0) // Example condition, adjust as needed
        {
            Debug.Log("All spheres collected! Door opened.");
        }
    }

    void modoInfinitoLogic()
    {
        sphereCount();
        if (spheres.Length == 0)
        {
            Debug.Log("Pontuação: " + pontuacaoModoInfinito);
            esferaScript.Instance.spawnSpheres();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentGameMode == GameMode.Infinito)
        {
            modoInfinitoLogic();
            HandleInfiniteTime();
        }
        else
        {
            sphereCount();
            openDoor();
        }

    }

    // Garante que a pontuação por tempo acumula mesmo que outras lógicas não sejam chamadas
    private void HandleInfiniteTime()
    {
        infiniteTimeAccumulator += Time.deltaTime;
        if (infiniteTimeAccumulator >= 1f)
        {
            int seconds = Mathf.FloorToInt(infiniteTimeAccumulator);
            pontuacaoModoInfinito += seconds;
            infiniteTimeAccumulator -= seconds;
            Debug.Log("Pontuação por tempo: +" + seconds + " (Total: " + pontuacaoModoInfinito + ")");
            UpdateScoreText();
        }
    }

    //update ball count text
    public void UpdateBallCountText(int count)
    {
        if (ballCountText != null)
        {
            if (scoreText == null)
            {
                ballCountText.text = "Balls Left: " + count.ToString() + "   Score: " + pontuacaoModoInfinito.ToString();
            }
            else
            {
                ballCountText.text = "Balls Left: " + count.ToString();
            }
        }
    }

        // Adiciona pontos (usado por scripts de coleta)
        public void AddScore(int amount)
        {
          pontuacaoModoInfinito += amount;
          Debug.Log("Pontuação por coleta: +" + amount + " (Total: " + pontuacaoModoInfinito + ")");
          UpdateScoreText();
        }

    // Atualiza o texto de pontuação na UI
    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + pontuacaoModoInfinito.ToString();
        }
        else if (ballCountText != null)
        {
            ballCountText.text = "Balls Left: " + currentBallCount.ToString() + "   Score: " + pontuacaoModoInfinito.ToString();
        }
        else
        {
            Debug.LogWarning("scoreText não está atribuído no GameManager.");
        }
    }
}
