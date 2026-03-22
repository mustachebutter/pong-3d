using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody rb;
    [SerializeField]
    private Ball ball;

    private bool bHasLaunchedBall = false;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move: {moveInput}");
        Vector3 move = new Vector3(moveInput.x, 0, 0).normalized;
        rb.linearVelocity = move * speed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!bHasLaunchedBall)
        {
            Debug.Log("LAUNCH!");
            ball.Launch();
            bHasLaunchedBall = true;
        }
    }
}
