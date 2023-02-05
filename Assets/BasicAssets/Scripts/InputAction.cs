using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputAction : MonoBehaviour
{
    public KeyCode keycode;
    public UnityEvent onInput;
    public bool onlyOnce = true;
    bool hasActivated = false;

    // Update is called once per frame
    void Update()
    {
        if(((onlyOnce && !hasActivated) || !onlyOnce) && InputHandler.GetGenericDown(keycode)){
            hasActivated = true;
            onInput.Invoke();
        }
    }
}
