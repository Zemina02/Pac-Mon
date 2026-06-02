
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCutscene : MonoBehaviour
{
    [Header("Referências")]
    public Transform playerCamera;
    public Transform enemy;

    [Header("Configuração da cutscene")]
    public float cameraTurnSpeed = 3f;
    public float enemyRiseAmount = 1f;
    public float enemyTiltAmount = 25f;
    public float enemyApproachDistance = 0.6f;
    public float cutsceneDuration = 2f;
    public string defeatSceneName = "GameOverScreen";

    private bool playing = false;
    private float timer = 0f;
    private Vector3 enemyStartPos;
    private Quaternion enemyStartRot;

    public void PlayCutscene()
    {
        playing = true;
        timer = 0f;

        // guardar posição inicial do inimigo
        enemyStartPos = enemy.position;
        enemyStartRot = enemy.rotation;

        // DESATIVAR MOVIMENTO DO PLAYER
        var move = FindObjectOfType<FirstPersonMovement>();
        if (move != null) move.enabled = false;

        // DESATIVAR ROTAÇÃO DA CÂMARA
        var look = FindObjectOfType<FirstPersonLook>();
        if (look != null) look.enabled = false;

        // DESATIVAR CAMERA CONTROL (C e troca de câmaras)
        var camControl = FindObjectOfType<CameraControl>();
        if (camControl != null) camControl.enabled = false;

        // PARAR FÍSICA DO PLAYER
        Rigidbody rb = FindObjectOfType<FirstPersonMovement>().GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    private void Update()
    {
        if (!playing) return;

        timer += Time.deltaTime;
        float t = timer / cutsceneDuration;

        // virar a câmara para o inimigo
        Vector3 dir = (enemy.position - playerCamera.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRot, Time.deltaTime * cameraTurnSpeed);

        // subir + inclinar + aproximar
        Vector3 risePos = enemyStartPos + Vector3.up * Mathf.Lerp(0, enemyRiseAmount, t);
        Vector3 approachPos = Vector3.Lerp(risePos, playerCamera.position + playerCamera.forward * enemyApproachDistance, t);

        enemy.position = approachPos;
        enemy.rotation = enemyStartRot * Quaternion.Euler(Mathf.Lerp(0, enemyTiltAmount, t), 0, 0);

        if (timer >= cutsceneDuration)
        {
            SceneManager.LoadScene(defeatSceneName);
        }
    }
}
