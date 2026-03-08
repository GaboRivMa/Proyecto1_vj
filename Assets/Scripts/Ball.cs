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


    [Header("Configuración de Apuntado")]
    public LineRenderer lineRenderer;
    private float customAngle = 90f; 
    private bool isAiming = false;

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
            //Para el funcionamiento de la fireball
            if (isAiming) {
                HandleAiming();
                // Lanza con Espacio o con el Clic Izquierdo del Mouse
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    LaunchDirected();
                }
            }
        }

        //reseteo de pelota
        if(Input.GetKeyDown(KeyCode.R)){
            ResetBall();
        }
    }

    // Método para activar el apuntado desde afuera
    public void EnableAiming() {
        if (FireballInventory.Instance != null && FireballInventory.Instance.currentFireballs > 0) {
            isAiming = true;
            if(lineRenderer != null) lineRenderer.enabled = true;
            customAngle = 90f; 
        }
    }

    /**
        Detección del mouse para la fireball
    **/
    void HandleAiming() {
        // Obtener la posición del mouse 
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; 
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calcular la dirección desde la bola hacia el mouse
        Vector3 direction = (worldMousePos - transform.position).normalized;
        direction.z = 0;

        // Limitar el ángulo (para que no dispare hacia abajo)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;    
        angle = Mathf.Clamp(angle, 20f, 160f);
        customAngle = angle; 

        // Actualizar la línea visual
        if (lineRenderer != null) {
            lineRenderer.SetPosition(0, transform.position);

            Vector3 restrictedDir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            lineRenderer.SetPosition(1, transform.position + restrictedDir * 2f);
        }
    }

    /**
        Lanzamiento dirigido con la fireball
    */
    public void LaunchDirected() {
    float radians = customAngle * Mathf.Deg2Rad;
    Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
    rb.velocity = direction.normalized * launchSpeed;
    
    launched = true;
    isAiming = false;
    if(lineRenderer != null) lineRenderer.enabled = false;

    // GASTAR LA FIREBALL
    if (FireballInventory.Instance != null) {
        FireballInventory.Instance.UseFireball();
    }
}
}
