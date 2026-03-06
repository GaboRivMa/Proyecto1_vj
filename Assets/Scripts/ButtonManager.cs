using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void GoToGame(){
        SceneManager.LoadScene("Juego");
    }

    public void GoToMenu(){
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("Menu");
    }

    public void GoToComoJugar(){
        SceneManager.LoadScene("Guia");
    }

    public void GoToMenuTuto(){
        SceneManager.LoadScene("Menu");
    }
}
