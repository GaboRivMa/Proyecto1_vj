using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{

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
                    //Debug.Log("PowerUp 1");
                    StartCoroutine(MultiBall());
                    break;
                case 1:
                    //Debug.Log("PowerUp 2");
                    StartCoroutine(ExtraSpeed());
                    break;
                case 2:
                    Debug.Log("FireBall");
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
        currentBall.gameObject.GetComponent<Ball>().MultiplySpeed(2);
        yield return new WaitForSeconds(3f);
        currentBall.gameObject.GetComponent<Ball>().DivideSpeed(2);

    }


}
