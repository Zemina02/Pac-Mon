using UnityEngine;

public class TitlePulse : MonoBehaviour
{
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * 2f) * 0.05f;
        transform.localScale = originalScale * scale;
    }
}
