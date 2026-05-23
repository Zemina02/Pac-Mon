using UnityEngine;

public class CollisaoEsfera : MonoBehaviour
{
    // Colisao da esfera com o jogador, quando o jogador colidir com a esfera, a esfera é destruida e a pontuação do modo infinito é incrementada em 1
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            GameManager.Instance.pontuacaoModoInfinito += 1;
        }
    }
}
