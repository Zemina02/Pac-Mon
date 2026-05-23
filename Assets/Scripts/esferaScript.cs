using UnityEngine;
using System.Collections;
using System;


public class esferaScript : MonoBehaviour
{
    public static esferaScript Instance;
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

    public GameObject esferaPrefab; // Prefab da esfera
    private GameObject[] esferas; // Variável para armazenar a esfera instanciada
    public void spawnSpheres()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Sphere_spawn");

        esferas = new GameObject[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject spawnPoint = spawnPoints[i];

            MeshRenderer meshRenderer = spawnPoint.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            GameObject newEsfera = Instantiate(
                esferaPrefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );

            newEsfera.transform.localScale = new Vector3(25, 25, 25);

            esferas[i] = newEsfera;
        }
    }

    
}
