using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void GoToGame(){
        SceneManager.LoadScene("Nivel1");
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

    public void GoToComoCredito(){
        SceneManager.LoadScene("Creditos");
    }
    
    public void Quit()
    {
        // Esto cierra el juego cuando ya está instalado (Build)
        Application.Quit();

        // Esto es opcional, solo para ver en la consola de Unity que funciona
        Debug.Log("El juego se está cerrando...");
    }
}
