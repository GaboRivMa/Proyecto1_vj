using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour{

    public float speed = 5f; 
    public Transform currentBall;
    public GameObject preFabBall;
    public int type = 0;

    // Start is called before the first frame update
    void Start(){
        currentBall = GameObject.FindGameObjectWithTag("Ball").transform;
        type = Random.Range(0,4);   
    }

    // Update is called once per frame
    void Update(){
        this.transform.Translate(Vector3.right * -1 * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){ 
            this.gameObject.GetComponent<Renderer>().enabled = false;
            switch(type){
                case 0: 
                    Debug.Log("Multiball");
                    StartCoroutine(MultiBall());
                    break;
                case 1:
                    Debug.Log("ExtraSpeed");
                    StartCoroutine(ExtraSpeed());
                    break;
                case 2:
                    Debug.Log("FireBall");
                    StartCoroutine(FireBall());
                    break;
                case 3:
                    Debug.Log("Movimiento cámara");
                    break;
            }
        }
        if(other.gameObject.tag=="DeadZone"){
            Destroy(this.gameObject);
        }
    }

    IEnumerator MultiBall(){
        var newBall1 = Instantiate(preFabBall, currentBall.position, Quaternion.identity);
        newBall1.GetComponent<Ball>().Launch();

        var newBall2 = Instantiate(preFabBall, currentBall.position, Quaternion.identity);
        newBall2.GetComponent<Ball>().Launch();

        //despues de un tiempo desaparecen las nuevas bolas
        yield return new WaitForSeconds(15f);

        Destroy(newBall1.gameObject);
        Destroy(newBall2.gameObject);
    }

    IEnumerator ExtraSpeed(){
        // Buscamos todas las bolas que existan en ese momento
        GameObject[] todasLasBolas = GameObject.FindGameObjectsWithTag("Ball");

        // Aceleramos todas
        foreach (GameObject bola in todasLasBolas) {
            Ball script = bola.GetComponent<Ball>();
            if (script != null) script.MultiplySpeed(2);
        }

        yield return new WaitForSeconds(3f);

        // Al terminar el tiempo, buscamos de nuevo (por si alguna se destruyó)
        GameObject[] bolasRestantes = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject bola in bolasRestantes) {
            Ball script = bola.GetComponent<Ball>();
            if (script != null) script.DivideSpeed(2);
        }

    }

    /**
        Mecánica de FireBall
    */
    IEnumerator FireBall() {
        Debug.Log("Limpiando clones y activando FireBall en la original...");

        //Busca todas las pelotas 
        GameObject[] todasLasBolas = GameObject.FindGameObjectsWithTag("Ball");

        //Elimina todas las que no son la principal
        foreach (GameObject ball in todasLasBolas) {
            if (ball.transform != currentBall) {
                Destroy(ball);
            }
        }

        //Prepara el tablero para la fireball
        if (currentBall != null) {
            Ball ballScript = currentBall.GetComponent<Ball>();

            if (ballScript != null) {
                //Resetea la pelota y su velocidad
                ballScript.ResetBall(); 
                ballScript.ResetSpeed();

                //Cambia el color a rojo
                Renderer render = currentBall.GetComponent<Renderer>();
                if (render != null) {
                    render.material.color = Color.red;
                }
            }
        }

        yield return null;
    }

}
