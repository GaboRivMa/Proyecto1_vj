using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager instance;

    public int score = 0;
    public int lives = 3;
    public Block[] blocks; 
    public Ball[] balls;

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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start(){
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
        balls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        ballCount = balls.Length;
    }

    void Update(){
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
        balls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        ballCount = balls.Length;
        CountBalls();
        scoreText.text = $"Puntos: {score}";
        livesText.text = $"Vidas: {lives}";
    }

    void CountBalls(){
        if(lives <= 0 || resetScreen.activeSelf)
            return;

        if(ballCount == 0)
            LoseLifes();
    }

    void RespawnBall(){
        Instantiate(ballPrefab, padTransform.position + new Vector3(0f, 0.65f, 0f), Quaternion.identity);
    }


    //logica de cuando destruimos bloques suba los puntos
    public void BlockDestroy(){
        blockCount--;
        score += 10;

        if(blockCount<=0){
            Debug.Log("You win! :)");
            EndGame();
        }
    }


    public void LoseLifes(){
        lives--;
        if(lives <=0){
            Debug.Log("You lose! :(");
            EndGame();
        }else{
            RespawnBall();
        }
        
    }

    public void EndGame(){
        GameObject.Find("Paddle").SetActive(false); 
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls){
            ball.SetActive(false);
        }
        resetScreen.SetActive(true);
    }

    public void ResetGame(){
        score = 0;
        lives = 3;
        gameScreen.SetActive(true);
        resetScreen.SetActive(false);
    }

}
