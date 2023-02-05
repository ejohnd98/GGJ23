using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public float cooldown = 0.25f;
    public bool oneShot = false;
    public string interactSound = "";

    public string flavorTown = "Interact";
    public string postInteractMsg = "";

    protected bool isCoolingDown = false;
    protected float counter = 0.0f;
    protected bool hasInteracted = false;

    public void InteractBase(){

        if(isCoolingDown){
            return;
        }

        if(interactSound.Length > 0){
            SoundSystem.PlaySoundStatic(interactSound, transform);
        }

        if(postInteractMsg.Length > 0){
            UISystem.ShowTextToPlayer(postInteractMsg);
        }

        isCoolingDown = true;
        counter = cooldown;
        Interact();
    }

    public bool CanInteract(){
        return !isCoolingDown && (!oneShot || !hasInteracted);
    }

    public virtual void Interact(){
        isCoolingDown = true;
        Debug.Log("Interact called on " + gameObject.name);
    }

    private void Update() {
        if(isCoolingDown && !UISystem.instance.UIInFocus){
            if(counter >= 0.0f){
                counter -= Time.deltaTime;
            }else{
                counter = 0.0f;
                isCoolingDown = false;
            }
        }
    }
}
