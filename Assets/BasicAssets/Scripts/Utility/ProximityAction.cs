using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityAction : MonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    public bool oneShot = true;
    
    bool hasTriggered = false;


    private void OnTriggerEnter(Collider other) {
        if(oneShot && hasTriggered){
            return;
        }
        OnEnter.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        if(oneShot && hasTriggered){
            return;
        }
        hasTriggered = true;
        OnExit.Invoke();
    }
}
