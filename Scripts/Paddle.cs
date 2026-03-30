using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private Vector3 moveInput;
    private Rigidbody rb;
    [SerializeField] private Ball ball;
    [SerializeField] private bool bIsPlayer = false;
    [SerializeField] private bool bIsSidePaddle = false;
    [SerializeField] private float speed = 50.0f;
    private bool bHasLaunchedBall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = Vector3.zero;
        
        if (bIsPlayer)
        {
            if(Input.GetKey(KeyCode.A))
            {
                moveInput = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = Vector3.right;
            }
        }
        else
        {
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = Vector3.right;
            }
        }

        Vector3 move = new Vector3((bIsSidePaddle ? -moveInput.x : moveInput.x) * speed, 0.0f, 0.0f);
        Vector3 globalMove = transform.TransformDirection(move);
        rb.linearVelocity = new Vector3(globalMove.x, 0.0f, globalMove.z);
    }

    void FixedUpdate()
    {
    }
}
