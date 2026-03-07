using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour{
    public float speed = 10f;
    public float minX = -6f;
    public float maxX = 6f;

    public Rigidbody rb;
    public float input;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        input = Input.GetAxis("Horizontal");
    }

    void FixedUpdate(){
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        rb.MovePosition(newPosition);
    }

}
