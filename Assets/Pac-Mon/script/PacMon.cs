using UnityEngine;
using UnityEngine.AI;

public class PacMon : MonoBehaviour
{
    private NavMeshAgent agent;
    private DeathCutscene deathCutscene;
    private bool cutsceneStarted = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        deathCutscene = FindObjectOfType<DeathCutscene>();
    }

    private void Update()
    {
        if (cutsceneStarted) return; // impedir movimento durante a cutscene

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(player.position);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !cutsceneStarted)
        {
            cutsceneStarted = true;

            // parar o inimigo imediatamente
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            // impedir física de empurrar o player
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // iniciar cutscene
            if (deathCutscene != null)
            {
                deathCutscene.enemy = transform;
                deathCutscene.PlayCutscene();
            }
        }
    }
}
