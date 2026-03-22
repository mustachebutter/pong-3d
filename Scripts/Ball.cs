using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    public float speed;
    bool bHasCollided = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        // Debug.Log(rb.linearVelocity);
    }

    public void Launch()
    {
        Vector3 direction = new Vector3(0, 0, -1.0f).normalized;
        rb.AddForce(direction * speed, ForceMode.VelocityChange);
    }
}
