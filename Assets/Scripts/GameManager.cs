using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    public static GameManager instance;

    public int score = 0;
    public int lives = 3;
    public Block[] blocks; 
    public GameObject[] balls;
    private bool isGameOver = false;

    public int blockCount =0;
    public int ballCount = 0;

    [Header("Bolas")]
    public GameObject ballPrefab;
    public Transform padTransform;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public GameObject resetScreen;
    public GameObject gameScreen;

    private void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        }
        else{
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start(){
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
        balls = GameObject.FindGameObjectsWithTag("Ball");
        ballCount = balls.Length;
    }

    void Update(){
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
        balls = GameObject.FindGameObjectsWithTag("Ball");
        ballCount = balls.Length;
        if(ballCount == 0 && !isGameOver)
            LoseLifes();
        scoreText.text = $"Puntos: {score}";
        livesText.text = $"Vidas: {lives}";
    }


    GameObject RespawnBall(){
        if (ballPrefab != null && padTransform != null && padTransform.gameObject.activeInHierarchy){
            GameObject newBall = Instantiate(ballPrefab, padTransform.position + new Vector3(0f, 0.65f, 0f), Quaternion.identity);
            return newBall; // La devolvemos
        }
        return null;
    }


    //logica de cuando destruimos bloques suba los puntos
    public void BlockDestroy(){
        blockCount--;
        score += 10;

        if(blockCount<=0){
            Debug.Log("You win! :)");
            NextLevel();
        }
    }


    public void LoseLifes(){
        lives--;
        if(lives <=0){
            Debug.Log("You lose! :(");
            EndGame();
        }else{
            GameObject ballGO = RespawnBall();

            // Si la bola se creó y tenemos Fireballs, activamos el apuntado
            if (ballGO != null && FireballInventory.Instance != null && FireballInventory.Instance.currentFireballs > 0) {
                Ball ballScript = ballGO.GetComponent<Ball>();
                if (ballScript != null) {
                    ballScript.EnableAiming(); 
                }
            }
        }
        
    }

    public void EndGame(){
        isGameOver = true;
        if(padTransform != null)
            padTransform.gameObject.SetActive(false);
        
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls){
            //ball.SetActive(false);
            Destroy(ball);
        }
        if(resetScreen != null)
            resetScreen.SetActive(true);
    }

    //Función para avanzar de nivel
    public void NextLevel(){
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentIndex == 3) {
            // Si terminó el último nivel, ve a los créditos
            SceneManager.LoadScene("Créditos"); 
        } else {
            // Si no, carga el que sigue por índice (Nivel1 -> Nivel2 -> Nivel3)
            SceneManager.LoadScene(currentIndex + 1);
        }
    }

    public void ResetGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
