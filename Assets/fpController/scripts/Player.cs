using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "PacMon")
        {
            transform.position = new Vector3(39, 1, 37);
        }

        // if (col.gameObject.tag == "Sphere")
        // {
        //     GameManager.Instance.pontuacaoModoInfinito++;
        // }
    }
}
