using UnityEngine;

public class CollisaoEsfera : MonoBehaviour
{
    public AudioClip collectSound;    
    private void OnTriggerEnter(Collider other) // Colisao da esfera com o jogador, quando o jogador colidir com a esfera, a esfera é destruida e a pontuação do modo infinito é incrementada em 1
    {
        if (other.gameObject.tag == "Player")
        { 
            AudioSource.PlayClipAtPoint(
                collectSound,
                transform.position
                //1f
            );
            Destroy(this.gameObject);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(10);
            }
        }
    }
}
