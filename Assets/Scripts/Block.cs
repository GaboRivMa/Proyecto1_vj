using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour{

    public GameObject capsule;

    public enum ColorMode {Random,Fijo}

    [Header("Configuración de Color")]
    [Tooltip("Random es la opción por defecto")]
    public ColorMode modoDeColor = ColorMode.Random; // Random por defecto
    
    public Color colorFijo = Color.white;

    [Header("Efecto de Brillo")]
    [Range(0f, 5f)]
    public float intensidadBrillo = 1.0f;
    public bool tieneBrillo = false;

    private Renderer rend;

    void AplicarApariencia(){
        if(rend == null)
            return;

        //Lógica de Color
        Color colorFinal;
        if(modoDeColor == ColorMode.Random){
            colorFinal = new Color(Random.value, Random.value, Random.value);
        }else{
            colorFinal = colorFijo;
        }

        //Lógica de Brillo (Emission)
        rend.material.color = colorFinal;

        if(tieneBrillo){
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", colorFinal * intensidadBrillo);
        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ball")){
            int posibility= Random.Range(0,10);
            if(posibility <= 10){//-----------------------------------------------------------------------
                Instantiate(capsule, this.transform.position,capsule.transform.rotation);
            }
            GameManager.instance.BlockDestroy();
            //AudioManager.Instance.PlayBlockSFX();
            Destroy(this.gameObject);
        }
    }

    private void Start(){
        rend = GetComponent<Renderer>();
        AplicarApariencia();
    }

}
