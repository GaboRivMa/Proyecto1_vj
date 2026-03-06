using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladrillo : MonoBehaviour
{

    public GameObject capsule;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            int posibility= Random.Range(0,10);
            if(posibility <= 4){
                Instantiate(capsule, this.transform.position,capsule.transform.rotation);
            }
            GameManager.instance.BlockDestroy();
            //Audio
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Start(){
        Color randomColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
        GetComponent<Renderer>().material.color = randomColor;
    }



}
