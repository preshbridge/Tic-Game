using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI[] buttonList;
    public TextMeshProUGUI title;
    private string playerSide;
    private int moveCount;
    public GameObject restartButton;

    public GameObject[] strikeThroughs;

    void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        title.text = "Player " + playerSide + " Turn";
        moveCount = 0;
        restartButton.SetActive(false);

        DisableAllStrikeThroughs();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<ButtonClickEvent>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        int winningCombination = CheckWinCondition();
        if (winningCombination != -1)
        {
            GameOver(winningCombination);
            return;
        }

        if (moveCount >= 9)
        {
            title.text = "It is a Draw ";
            restartButton.SetActive(true);
            return;
        }

        ChangePlayer();
    }

    void GameOver(int winningCombination)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
        title.text = "Player " + playerSide + " Wins";
        restartButton.SetActive(true);

        ShowStrikeThrough(winningCombination);
    }

    void ChangePlayer()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        title.text = "Player " + playerSide + " Turn";
    }

    public void RestartGame()
    {
        restartButton.SetActive(false);
        playerSide = "X";
        moveCount = 0;
        title.text = "Player " + playerSide + " Turn";

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = true;
            buttonList[i].text = "";
        }

        DisableAllStrikeThroughs();
    }

    int CheckWinCondition()
    {
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            return 0; // Top row
        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            return 1; // Middle row
        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            return 2; // Bottom row
        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            return 3; // Left column
        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            return 4; // Middle column
        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            return 5; // Right column
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            return 6; // Diagonal top-left to bottom-right
        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            return 7; // Diagonal top-right to bottom-left

        return -1; // No win
    }

    void ShowStrikeThrough(int winningCombination)
    {
        if (strikeThroughs.Length > winningCombination && strikeThroughs[winningCombination] != null)
        {
            strikeThroughs[winningCombination].SetActive(true);
        }
    }

    void DisableAllStrikeThroughs()
    {
        foreach (GameObject strikeThrough in strikeThroughs)
        {
            if (strikeThrough != null)
            {
                strikeThrough.SetActive(false);
            }
        }
    }
}
