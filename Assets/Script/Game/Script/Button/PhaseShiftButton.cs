using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseShiftButton : GameButtonBase {

    private Text ButtonText;

    private string searchtext = "Search";
    private string backtohome = "Back";
    private string enemytext = "Enemy";

    protected override void Start()
    {
        base.Start();
        ButtonText = gameObject.GetComponentInChildren<Text>();
        AddButtonClickEvent(SwitchPhase);
    }

    private void SwitchPhase()
    {
        ManagerHandler.Instance.NetworkManager().SendMessageFromPhaseShiftButton();
    }

    public void ChangeSearchButtonText(PlayerSearchState currentState)
    {
        if (currentState.Equals(PlayerSearchState.BACKTOHOME))
        {
            ButtonText.text = searchtext;
        }
        else if (currentState.Equals(PlayerSearchState.SHEEPSEARCH))
        {
            ButtonText.text = enemytext;
        }
        else if (currentState.Equals(PlayerSearchState.ENEMYSEARCH))
        {
            ButtonText.text = backtohome;
        }
    }

}
