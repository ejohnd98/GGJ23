using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material secondaryMat;
    public GameObject origObjs, swappedObjs;

    MeshRenderer meshRenderer;
    Material originalMat;
    bool hasSwapped = false;
    

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMat = meshRenderer.material;

        SetObjState(!hasSwapped);
    }

    public void SwapMaterials(){
        meshRenderer.material = hasSwapped ? originalMat : secondaryMat;
        hasSwapped = !hasSwapped;

        SetObjState(!hasSwapped);
    }

    public void SetObjState(bool originalState){
        if(origObjs != null){
            origObjs.SetActive(originalState);
        }
        if(swappedObjs != null){
            swappedObjs.SetActive(!originalState);
        }
    }
}
