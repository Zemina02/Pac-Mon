using NUnit.Framework;
using UnityEngine;

public class Walls_spawn : MonoBehaviour
{
    public static Walls_spawn Instance;

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
    public GameObject wallGeneralPrefab; // Prefab da parede boa para qualquer posição
    public GameObject wallCondicionadaPrefab; // Prefab da parede que tem de ser colocada em posições específicas para os Cantos (Rodar 90º para os cantos esquerdos e -90º para os cantos direitos)
    public GameObject wallAntiCantos; // Prefab da parede que nao pode ir aos cantos


    public void SpawnWalls()
    {
        int randomWallType;
        // Encotrar as posiçoes de spawn
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Wall_Spawn");
        // Desativar a MeshRenderer dos pontos de spawn para que não sejam visíveis
        foreach (GameObject spawnPoint in spawnPoints)
        {
            MeshRenderer meshRenderer = spawnPoint.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
        // Para cada posiçao de spawn, escolher um tipo de parede aleatoriamente e instanciá-la
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.name.Contains("Canto_TOP_E") ||
                spawnPoint.name.Contains("Canto_BOTTOM_E") ||
                spawnPoint.name.Contains("Canto_TOP_D") ||
                spawnPoint.name.Contains("Canto_BOTTOM_D"))
            {
                randomWallType = Random.Range(0, 2);
            }
            else
            {
                randomWallType = Random.Range(0, 3);
            }

            GameObject wallToSpawn;

            if (randomWallType == 0)
            {
                //
                wallToSpawn = wallGeneralPrefab;
                Instantiate(wallToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
            else if (randomWallType == 1)
            {
                // Verificar se a posição é um canto (pelo nome do objeto) fazer uma rotação de 90º se for um canto esquerdo ou -90º se for um canto direito
                if (spawnPoint.name.Contains("Canto_TOP_E") || spawnPoint.name.Contains("Canto_BOTTOM_E"))
                {
                    wallToSpawn = wallCondicionadaPrefab;
                    GameObject wallInstance = Instantiate(wallToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    wallInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (spawnPoint.name.Contains("Canto_TOP_D") || spawnPoint.name.Contains("Canto_BOTTOM_D"))
                {
                    wallToSpawn = wallCondicionadaPrefab;
                    GameObject wallInstance = Instantiate(wallToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    wallInstance.transform.rotation = Quaternion.Euler(0, -90, 0);
                    //mover um pouco a parede para o lado para que fique bem posicionada no canto( mais afastada do canto para nao bloquear o caminho do jogador)
                    wallInstance.transform.position += Vector3.left * 10.5f; 
                }
                //se nao for um canto, instanciar a parede normalmente
                else
                {
                    wallToSpawn = wallCondicionadaPrefab;
                    Instantiate(wallToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
                }

            }
            else if (randomWallType == 2)
            {
                wallToSpawn = wallAntiCantos;
                Instantiate(wallToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }

        }

    }
}
