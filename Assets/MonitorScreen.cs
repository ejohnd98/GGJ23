using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorScreen : MonoBehaviour
{
    MeshRenderer meshRenderer;

    public Texture2D[] monitorTextures;
    int currentIndex = 0;
    float counter = 1.0f;
    public float switchTime = 5.0f;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += (Time.deltaTime / switchTime);
        if(counter >= 1.0f){
            counter = 0.0f;

            meshRenderer.material.mainTexture = monitorTextures[currentIndex];
            currentIndex = (currentIndex+1) % monitorTextures.Length;
        }
    }
}
