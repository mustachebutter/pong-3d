using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition = Vector3.zero;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;      
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

    public void Launch(bool isTowardsPlayer)
    {
        Vector3 direction = Vector3.zero;
        float[] xOptions = { 0.5f, -0.5f, 0.3f, -0.3f };
        int choice = Random.Range(0, xOptions.Length - 1);

        direction = new Vector3(xOptions[choice], 0, isTowardsPlayer ? 1.0f : -1.0f).normalized;
        rb.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    public void ResetBall()
    {
        transform.position = originalPosition;
    }
}
