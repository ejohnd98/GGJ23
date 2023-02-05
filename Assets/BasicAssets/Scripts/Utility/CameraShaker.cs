using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Animator animator;
    float currentShake = 0.0f;
    public float recoveryTime = 10.0f;

    public float testInput = 1.0f;
    public bool testTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentShake > 0){
            currentShake = Mathf.Lerp(currentShake, 0, Time.deltaTime * recoveryTime);
            animator.SetFloat("Shake", currentShake);
        }
        if(testTrigger){
            testTrigger = false;
            ShakeCamera(testInput);
        }
    }

    public void ShakeCamera(float intensity){
        currentShake = intensity;
    }
}
