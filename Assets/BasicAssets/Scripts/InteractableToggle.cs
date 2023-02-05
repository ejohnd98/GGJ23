using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableToggle : InteractableBase
{
    public UnityEvent enableActions;
    public UnityEvent disableActions;

    public bool active = false;

    public override void Interact(){
        active = !active;
        if(active){
            enableActions.Invoke();
        }else{
            disableActions.Invoke();
        }
    }
}
