using UnityEngine;

public class GhostFloat : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos +
            Vector3.up * Mathf.Sin(Time.time * 2f) * 0.2f;

        transform.Rotate(0, 20 * Time.deltaTime, 0);
    }
}