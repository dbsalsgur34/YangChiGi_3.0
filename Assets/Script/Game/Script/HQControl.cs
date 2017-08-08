using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerHerdSheepControl))]

public class HQControl : MonoBehaviour {

    private PlayerHerdSheepControl HQHerdControl;
    private PlayerControlThree owner;
    public SpriteRenderer HQMarker;

    private void Awake()
    {
        HQHerdControl = GetComponent<PlayerHerdSheepControl>();
    }

    public PlayerHerdSheepControl GetHQHerd()
    {
        return this.HQHerdControl;
    }

    public void SetOwner(PlayerControlThree owner)
    {
        this.owner = owner;
        HQHerdControl.InitHerdSheepBase(owner, 0, false);
    }

    public void SetHQMarkerColor(Color targetColor)
    {
        HQMarker.color = targetColor;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (GameTime.IsTimerStart())
        {
            var target = col.gameObject.GetComponent<PlayerControlThree>();
            if (target != null && target == this.owner)
            {
                target.GetPlayerState().InHQ = true;
            }

            var targetHerd = col.gameObject.GetComponent<HerdSheepBase>();
            if (targetHerd != null && targetHerd.GetOwner() == this.owner)
            {
                StartCoroutine(targetHerd.MoveAllSheepToTarget(this.HQHerdControl));
            }
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (GameTime.IsTimerStart())
        {
            var target = col.gameObject.GetComponent<PlayerControlThree>();
            if (target != null)
            {
                target.GetPlayerState().InHQ = false;
            }
        }
    }

}
