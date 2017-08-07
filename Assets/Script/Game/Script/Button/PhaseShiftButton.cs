using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseShiftButton : GameButtonBase {

    private Text ButtonText;
    private Image ButtonImage;
    private string searchtext = "Search";
    private string backtohome = "Home";
    private string enemytext = "Enemy";

    private Sprite targetHome;
    private Sprite targetEnemy;
    private Sprite targetSheep;

    protected override void Start()
    {
        base.Start();
        ButtonText = gameObject.GetComponentInChildren<Text>();
        ButtonImage = GetComponent<Image>();
        AddButtonClickEvent(SwitchPhase);

        targetHome = Resources.Load<Sprite>("Image/Resource/Button/Black/TargetHome");
        targetEnemy = Resources.Load<Sprite>("Image/Resource/Button/Black/TargetEnemy");
        targetSheep = Resources.Load<Sprite>("Image/Resource/Button/Black/TargetSheep");
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
            ButtonImage.sprite = targetSheep;
        }
        else if (currentState.Equals(PlayerSearchState.SHEEPSEARCH))
        {
            ButtonText.text = enemytext;
            ButtonImage.sprite = targetEnemy;
        }
        else if (currentState.Equals(PlayerSearchState.ENEMYSEARCH))
        {
            ButtonText.text = backtohome;
            ButtonImage.sprite = targetHome;
        }
        AudioManager.Instance.PlayOneShotEffectClipByName("Button_InGame_Tick");
    }

}
