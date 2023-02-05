using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{

    public GameObject cameraObj;
    public LayerMask interactLayer;
    public float interactDistance = 3.0f;

    bool promptShowing = false;
    string flavorTown = "";

    // Update is called once per frame
    void Update()
    {
        Ray ray = cameraObj.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        bool showPrompt = false;

        if(!UISystem.instance.UIInFocus){
            if (Physics.Raycast(ray, out hit, interactDistance, interactLayer)){

                InteractableBase interactable;
                if(interactable = hit.collider.GetComponent<InteractableBase>()){

                    if(interactable.enabled && interactable.CanInteract()){
                        showPrompt = true;
                        flavorTown = interactable.flavorTown;

                        if(InputHandler.GetInteractButtonDown()){
                            interactable.InteractBase();
                            showPrompt = false;
                        }
                    }
                }
            }
        }

        if(promptShowing != showPrompt){
            UISystem.SetInteractPrompt(flavorTown, showPrompt);
            promptShowing = showPrompt;
        }
    }
}
