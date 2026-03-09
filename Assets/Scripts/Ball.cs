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

    [Header("Fireball")]
    public bool isFireMode = false;
    public float fireDuration = 3f;
    private Coroutine fireRoutine;

    void Awake(){
        rb = GetComponent<Rigidbody>();

        GameObject paddle = GameObject.Find("Paddle");
        if (paddle != null)
            padTransform = paddle.transform;

        if(lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
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
            //Para el funcionamiento de la fireball
            if (isAiming) {
                HandleAiming();
                // Lanza con Espacio o con el Clic Izquierdo del Mouse
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    LaunchDirected();
                }
            }
            else if(Input.GetKeyDown(KeyCode.Space)){
                Launch();
            }
        }

        //reseteo de pelota
        if(Input.GetKeyDown(KeyCode.R)){
            ResetBall();
        }
    }

    // Este método para no rebotar con los bloques cuando se tiene la fireball
    private void OnCollisionEnter(Collision collision) {
        if (isFireMode && collision.gameObject.CompareTag("Block")) {
            rb.velocity = lastVelocity;            
        }
    }

    private Vector3 lastVelocity;
    void FixedUpdate() {
        lastVelocity = rb.velocity;
    }
    
    public void ActivateFireballPower() {
        if (fireRoutine != null) {
            StopCoroutine(fireRoutine);
        }
        fireRoutine = StartCoroutine(FireballRoutine());
    }
    
    private IEnumerator FireballRoutine() {
        isFireMode = true;
        GetComponent<Renderer>().material.color = Color.red;        
        yield return new WaitForSeconds(fireDuration);
        isFireMode = false;
        GetComponent<Renderer>().material.color = Color.white; // Vuelve a la normalidad
        fireRoutine = null;
    }

    public void LaunchDirected() {
        float radians = customAngle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        rb.velocity = direction.normalized * launchSpeed;

        launched = true;
        isAiming = false;
        if(lineRenderer != null) lineRenderer.enabled = false;

        ActivateFireballPower();

        if (FireballInventory.Instance != null) {
            FireballInventory.Instance.UseFireball();
        }
    }

    // Método para activar el apuntado desde afuera
    public void EnableAiming() {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        if (FireballInventory.Instance != null && FireballInventory.Instance.currentFireballs > 0) {
            isAiming = true;
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = Color.red;

            if(lineRenderer != null){
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;   // ← MUY IMPORTANTE
            }
            customAngle = 90f;
        }
        
    }

    
    void HandleAiming() {

        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mouse);
        worldMouse.z = 0;

        Vector3 dir = (worldMouse - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, 20f, 160f);

        customAngle = angle;

        if(lineRenderer != null){

            lineRenderer.SetPosition(0, transform.position);

            Vector3 visualDir =
                new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
                            Mathf.Sin(angle * Mathf.Deg2Rad),
                            0);

            lineRenderer.SetPosition(1, transform.position + visualDir * 3f);
        }
    }

}