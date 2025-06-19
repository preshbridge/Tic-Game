using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartPlayerVsPlayer()
    {
        SceneManager.LoadScene("TicTacToe"); 
    }

    public void StartPlayerVsComputer()
    {
        SceneManager.LoadScene("Human vs Computer"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
