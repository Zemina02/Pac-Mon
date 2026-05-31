using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PacMon : MonoBehaviour
{
    private Vector3 spawnPoint = new Vector3();
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
    }

    private void Update()
    {
        agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
             
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        }
    }
}