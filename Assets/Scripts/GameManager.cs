using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode { Normal, Infinito };
    public GameMode currentGameMode;
    public int pontuacaoModoInfinito;

    GameObject[] spheres;
    GameObject player;
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
        }
    }

    void sphereCount()
    {
        spheres = GameObject.FindGameObjectsWithTag("Spheres");
        Debug.Log("Number of spheres: " + spheres.Length);
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
        }
        else
        {
            sphereCount();
            openDoor();
        }

    }
}
