using UnityEngine;

public class FogZone : MonoBehaviour
{
    [Header("Nevoeiro alvo nesta zona")]
    public float targetFogDensity = 0.05f;
    public Color targetFogColor = Color.gray;
    public float transitionSpeed = 1f;

    private float originalFogDensity;
    private Color originalFogColor;
    private bool playerInside = false;

    void Start()
    {
        originalFogDensity = RenderSettings.fogDensity;
        originalFogColor = RenderSettings.fogColor;
    }

    void Update()
    {
        if (playerInside)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * transitionSpeed);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, Time.deltaTime * transitionSpeed);
        }
        else
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, originalFogDensity, Time.deltaTime * transitionSpeed);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, originalFogColor, Time.deltaTime * transitionSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
