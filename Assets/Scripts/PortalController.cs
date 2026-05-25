using UnityEngine;

namespace game
{
    public class PortalController : MonoBehaviour
    {
        public GameObject[] spheres;
        Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void sphereCount()
        {
            spheres = GameObject.FindGameObjectsWithTag("Spheres");
            Debug.Log("Number of spheres: " + spheres.Length);
        }

        void openDoor()
        {
            int sphereCount = spheres.Length;

            if (sphereCount == 0 || sphereCount == null)
            {
                animator.SetBool("isOpen", true);
            }
            else
            {
                animator.SetBool("isOpen", false);
            }
        }

        void Update()
        {
            sphereCount();
            openDoor();
        }
    }
}
