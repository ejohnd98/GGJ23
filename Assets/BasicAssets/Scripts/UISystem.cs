using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystem : MonoBehaviour
{
    public static UISystem instance;

    public Transform promptParent, interactPrompt, textPrompt;
    public bool UIInFocus = false;
    string currentUIFocus;
    bool interactInputBuffer = false;

    Dictionary<string, Transform> promptDictionary;

    private void Awake() {
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else{
            instance = this;
        }

        promptDictionary = new Dictionary<string, Transform>();

        for(int i = 0; i < promptParent.childCount; i++){
            Transform child = promptParent.GetChild(i);
            promptDictionary.Add(child.name, child);
        }
    }

    public static void SetInteractPrompt(string flavorTown, bool visible){
        instance.interactPrompt.GetComponent<TMP_Text>().text = "[E] - " + flavorTown;
        instance.interactPrompt.gameObject.SetActive(visible);
    }

    public static void SetPromptVisible(string promptName, bool visible){
        if(instance.promptDictionary.ContainsKey(promptName)){
            instance.promptDictionary[promptName].gameObject.SetActive(visible);
        }else{
            Debug.LogWarning("UI does not contain prompt transform: " + promptName);
        }
    }

    public void ShowTransformToPlayer(string name){
        currentUIFocus = name;
        SetPromptVisible(currentUIFocus, true);
        FocusOnUI();
    }

    public static void ShowTextToPlayer(string text){
        if(instance.textPrompt.gameObject.activeSelf){
            instance.textPrompt.gameObject.SetActive(false);
            instance.textPrompt.gameObject.SetActive(true);
        }
        instance.textPrompt.GetComponent<TMP_Text>().text = text;
        instance.textPrompt.gameObject.SetActive(true);
    }

    private void Update() {
        if(interactInputBuffer){
            interactInputBuffer = false;
            return;
        }
        if(UIInFocus && currentUIFocus.Length > 0 && (InputHandler.GetInteractButtonDown() || InputHandler.GetFireButtonDown())){
            SetPromptVisible(currentUIFocus, false);
            currentUIFocus = "";
            ReturnFocusToPlayer();
        }
    }


    public static void FocusOnUI(){
        Cursor.lockState = CursorLockMode.None;
        GlobalUtil.FreezePlayerPosition(true);
        GlobalUtil.FreezePlayerRotation(true);
        instance.UIInFocus = true;
        instance.interactInputBuffer = true;
    }

    public static void UnlockCursor(){
        Cursor.lockState = CursorLockMode.None;
    }

    public static void ReturnFocusToPlayer(){
        Cursor.lockState = CursorLockMode.Locked;
        GlobalUtil.FreezePlayerPosition(false);
        GlobalUtil.FreezePlayerRotation(false);
        instance.UIInFocus = false;
    }

}
