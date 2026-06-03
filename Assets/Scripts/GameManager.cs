
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
            MakeMeshesReadable();
            navMeshSurface.BuildNavMesh();  
        }
    }

    // Ensure meshes used as NavMesh sources are readable at runtime
    void MakeMeshesReadable()
    {
        MeshFilter[] mfs = FindObjectsOfType<MeshFilter>();
        foreach (var mf in mfs)
        {
            Mesh src = mf.sharedMesh;
            if (src == null) continue;
            bool readable = true;
            try { readable = src.isReadable; } catch { readable = false; }
            if (readable) continue;

            try
            {
                Mesh newMesh = new Mesh();
                newMesh.name = src.name + "_copy";

                // copy vertex data
                newMesh.vertices = src.vertices;

                int subCount = src.subMeshCount;
                newMesh.subMeshCount = subCount;
                for (int s = 0; s < subCount; s++)
                {
                    newMesh.SetTriangles(src.GetTriangles(s), s);
                }

                if (src.normals != null && src.normals.Length == src.vertexCount) newMesh.normals = src.normals;
                if (src.tangents != null && src.tangents.Length == src.vertexCount) newMesh.tangents = src.tangents;
                if (src.colors != null && src.colors.Length == src.vertexCount) newMesh.colors = src.colors;
                if (src.uv != null && src.uv.Length == src.vertexCount) newMesh.uv = src.uv;
                if (src.uv2 != null && src.uv2.Length == src.vertexCount) newMesh.uv2 = src.uv2;

                // skinning data
                try { newMesh.bindposes = src.bindposes; } catch { }
                try { newMesh.boneWeights = src.boneWeights; } catch { }

                newMesh.RecalculateBounds();

                // assign to filter and related components
                mf.sharedMesh = newMesh;

                var mc = mf.GetComponent<MeshCollider>();
                if (mc != null) mc.sharedMesh = newMesh;

                var smr = mf.GetComponent<SkinnedMeshRenderer>();
                if (smr != null && smr.sharedMesh == src) smr.sharedMesh = newMesh;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to deep-clone mesh {src.name}: {e.Message}");
            }
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
