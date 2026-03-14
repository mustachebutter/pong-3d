using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody rb;
    [SerializeField]
    private Ball ball;

    private bool bHasLaunchedBall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0).normalized;
        // transform.Translate(move * 50.0f * Time.deltaTime);
        rb.linearVelocity = move * 50.0f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move: {moveInput}");
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
