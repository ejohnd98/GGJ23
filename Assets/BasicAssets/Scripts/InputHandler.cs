using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    //Gamepad gamepad;
    //Keyboard keyboard;
    
    static public float GetHorizontalAxis(){
        float value = 0;
        value += Input.GetKey(KeyCode.D)? 1.0f : 0.0f;
        value -= Input.GetKey(KeyCode.A)? 1.0f : 0.0f;
        return value;
    }

    static public float GetVerticalAxis(){
        float value = 0;
        value += Input.GetKey(KeyCode.W)? 1.0f : 0.0f;
        value -= Input.GetKey(KeyCode.S)? 1.0f : 0.0f;
        return value;
    }

    static public float GetLookAxis(){
        float value = 0;
        value += Input.GetKey(KeyCode.RightArrow)? 1.0f : 0.0f;
        value -= Input.GetKey(KeyCode.LeftArrow)? 1.0f : 0.0f;
        return value;
    }

    static public bool GetFireButtonDown(){
        return Input.GetMouseButtonDown(0);
    }

    static public bool GetAltFireButtonDown(){
        return Input.GetMouseButtonDown(1);
    }

    static public bool GetInteractButtonDown(){
        return Input.GetKeyDown(KeyCode.E);
    }

    static public bool GetFireButtonHeld(){
        return Input.GetMouseButton(0);
    }

    static public bool GetAltFireButtonHeld(){
        return Input.GetMouseButton(1);
    }

    static public bool GetGenericDown(KeyCode keycode){
        return Input.GetKeyDown(keycode);
    }
}
