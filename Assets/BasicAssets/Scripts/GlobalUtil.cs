using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUtil : MonoBehaviour
{
    public static GlobalUtil instance;
    public PlayerController player;

    List<string> progressKeys;

    private void Awake() {
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else{
            instance = this;
        }

        player = FindObjectOfType<PlayerController>();
        progressKeys = new List<string>();
    }

    public static void FreezePlayerPosition(bool isFrozen){
        instance.player.freezePosition = isFrozen;
    }

    public static void FreezePlayerRotation(bool isFrozen){
        instance.player.freezeRotation = isFrozen;
    }

    public static void AddProgressKey(string key){
        if(!HasProgressKey(key)){
            instance.progressKeys.Add(key);
        }
    }

    public static void RemoveProgressKey(string key){
        if(HasProgressKey(key)){
            instance.progressKeys.Remove(key);
        }
    }

    public static bool HasProgressKey(string key){
        return instance.progressKeys.Contains(key);
    }

    public static void ClearProgressKeys(){
        instance.progressKeys.Clear();
    }
}
