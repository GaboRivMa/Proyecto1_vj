using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float minX = -6f;
    public float maxX = 6f;

    public Rigidbody rb;
    public float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxis("Horizontal");   
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        rb.MovePosition(newPosition);
    }
}
