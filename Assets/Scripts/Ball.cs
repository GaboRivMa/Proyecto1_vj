using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float launchSpeed = 8f;
    public bool launched = false;
    private Rigidbody rb;

    [Header("Referencia al pad")] //inspector va a aparecer un letrero
    public Transform padTransform;
    public Vector3 offsetFromPad = new Vector3(0f,0.65f,0f); 


    void Awake(){
        rb = GetComponent<Rigidbody>();

        GameObject paddle = GameObject.Find("Paddle");
        if (paddle != null)
            padTransform = paddle.transform;
    }


    public void Launch(){
        //Lanzamiento en diagonal
        float angle = Random.Range(10f,170f);
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians),0f);
        rb.velocity = direction.normalized * launchSpeed;
        launched = true;
    }

    void FollowPad(){
        transform.position = padTransform.position + offsetFromPad;
    }

    public void ResetBall(){
        launched = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ResetSpeed();
        GetComponent<Renderer>().material.color = Color.white;
        FollowPad();
    }

    public void ResetSpeed(){
        launchSpeed = 8f;
        if (launched && rb != null) {
            // Aplicamos la velocidad base a la dirección actual
            rb.velocity = rb.velocity.normalized * launchSpeed;
        }
    }

    public void MultiplySpeed(int multi){
        launchSpeed = launchSpeed * multi;

        if (launched && rb != null) {
            // Mantenemos la dirección (normalized) y le damos la nueva magnitud
            rb.velocity = rb.velocity.normalized * launchSpeed;
        }
    }

    public void DivideSpeed(int div){
        launchSpeed = launchSpeed / div;
        if (launched && rb != null) {
            rb.velocity = rb.velocity.normalized / launchSpeed;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("DeadZone")){
            Destroy(this.gameObject);
        }
    }

    void Update(){
        if(!launched){
            FollowPad();
            if(Input.GetKeyDown(KeyCode.Space)){
                Launch();
            }
        }

        //reseteo de pelota
        if(Input.GetKeyDown(KeyCode.R)){
            ResetBall();
        }
    }
}
