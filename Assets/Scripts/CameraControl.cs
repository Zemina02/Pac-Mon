using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float camera;
   
    public static CameraControl Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        camera = Input.GetAxisRaw("Camera");
    }

    public void cameraSwitch()
    {
        if (camera > 0)
        {
            ShowOverheadView();
        }
        else
        {
            ShowFirstPersonView();
        }
}
    public Camera firstPersonCamera;
    public Camera overheadCamera;

    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView() {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
        Debug.Log("Camera Switch");
    }
    
    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView() {
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
        Debug.Log("Camera Switch");
    }

   private void Update()
{
    if (Input.GetKeyDown(KeyCode.C))
    {
        if (firstPersonCamera.enabled)
        {
            ShowOverheadView();
        }
        else
        {
            ShowFirstPersonView();
        }
    }
}
}

