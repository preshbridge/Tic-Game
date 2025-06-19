using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonClickEvent : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    private GameController gameController;
    private GameController1 computerController;

    public void SetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn();
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    public void SetComputerControllerReference(GameController1 controller)
    {
        computerController = controller;
    }

    public void SetComputerSpace()
    {
        buttonText.text = computerController.GetPlayerSide();
        button.interactable = false;
        computerController.EndTurn();
    }
}
