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

    public int blockCount =0;

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

    }

    void Update(){
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
        scoreText.text = $"Puntos: {score}";
        livesText.text = $"Vidas: {lives}";
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
