using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : InteractableBase
{
    public UnityEvent actions;

    public override void Interact(){
        if(oneShot && hasInteracted){
            return;
        }
        hasInteracted = true;
        actions.Invoke();
    }
}
