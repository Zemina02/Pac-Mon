using UnityEngine;

public class PortalVitoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Vitoria();
        }
    }
}
