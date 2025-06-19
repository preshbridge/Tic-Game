using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController1 : MonoBehaviour
{
    public TextMeshProUGUI[] buttonList;
    public TextMeshProUGUI title;
    private string playerSide;
    private int moveCount;
    public GameObject restartButton;
    private bool computerTurn = false;
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
            buttonList[i].GetComponentInParent<ButtonClickEvent>().SetComputerControllerReference(this);
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
            title.text = "It is a Draw";
            restartButton.SetActive(true);
            return;
        }

        ChangePlayer();

        if (computerTurn && playerSide == "O")
        {
            StartCoroutine(ComputerMoveWithDelay());
        }
    }

    void ChangePlayer()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        title.text = "Player " + playerSide + " Turn";
        computerTurn = (playerSide == "O"); 
    }

    IEnumerator ComputerMoveWithDelay()
    {
        yield return new WaitForSeconds(1f); 

        int bestMove = GetBestMove();
        buttonList[bestMove].GetComponentInParent<ButtonClickEvent>().SetComputerSpace();
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

    int Minimax(TextMeshProUGUI[] newBoard, int depth, bool isMaximizing)
    {
        int score = EvaluateBoard();

        if (score == 10) return score - depth;
        if (score == -10) return score + depth;
        if (!IsMovesLeft(newBoard)) return 0;

        if (isMaximizing)
        {
            int best = -1000;
            for (int i = 0; i < newBoard.Length; i++)
            {
                if (newBoard[i].text == "")
                {
                    newBoard[i].text = "O";
                    best = Mathf.Max(best, Minimax(newBoard, depth + 1, false));
                    newBoard[i].text = "";
                }
            }
            return best;
        }
        else
        {
            int best = 1000;
            for (int i = 0; i < newBoard.Length; i++)
            {
                if (newBoard[i].text == "")
                {
                    newBoard[i].text = "X";
                    best = Mathf.Min(best, Minimax(newBoard, depth + 1, true));
                    newBoard[i].text = "";
                }
            }
            return best;
        }
    }

    int EvaluateBoard()
    {
        for (int row = 0; row < 3; row++)
        {
            if (buttonList[row * 3].text == buttonList[row * 3 + 1].text &&
                buttonList[row * 3 + 1].text == buttonList[row * 3 + 2].text)
            {
                if (buttonList[row * 3].text == "O") return 10;
                if (buttonList[row * 3].text == "X") return -10;
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (buttonList[col].text == buttonList[col + 3].text &&
                buttonList[col + 3].text == buttonList[col + 6].text)
            {
                if (buttonList[col].text == "O") return 10;
                if (buttonList[col].text == "X") return -10;
            }
        }

        if (buttonList[0].text == buttonList[4].text && buttonList[4].text == buttonList[8].text)
        {
            if (buttonList[0].text == "O") return 10;
            if (buttonList[0].text == "X") return -10;
        }

        if (buttonList[2].text == buttonList[4].text && buttonList[4].text == buttonList[6].text)
        {
            if (buttonList[2].text == "O") return 10;
            if (buttonList[2].text == "X") return -10;
        }

        return 0;
    }

    bool IsMovesLeft(TextMeshProUGUI[] newBoard)
    {
        for (int i = 0; i < newBoard.Length; i++)
        {
            if (newBoard[i].text == "") return true;
        }
        return false;
    }

    int GetBestMove()
    {
        int bestVal = -1000;
        int bestMove = -1;

        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "")
            {
                buttonList[i].text = "O"; 
                int moveVal = Minimax(buttonList, 0, false); 
                buttonList[i].text = ""; 

                if (moveVal > bestVal)
                {
                    bestMove = i;
                    bestVal = moveVal;
                }
            }
        }
        return bestMove;
    }

    void GameOver(int winningCombination)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
        title.text = "Player " + playerSide + " Wins!";
        restartButton.SetActive(true);

        ShowStrikeThrough(winningCombination);
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

    public void RestartGame()
    {
        restartButton.SetActive(false);
        playerSide = "X";
        moveCount = 0;
        title.text = "Player " + playerSide + " Turn";
        computerTurn = false;

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = true;
            buttonList[i].text = "";
        }

        DisableAllStrikeThroughs();
    }
}
