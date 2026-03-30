using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;
    public readonly float[] CAMERA_HEIGHT = new float[]
    {
        20.0f,
        30.0f,
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = gameObject;
        SetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraPosition()
    {
        int currentActiveScene = SceneManager.GetActiveScene().buildIndex;
        transform.position = new Vector3(transform.position.x, CAMERA_HEIGHT[currentActiveScene], transform.position.z);
    }
}
