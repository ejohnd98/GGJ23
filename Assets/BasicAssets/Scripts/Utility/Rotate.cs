using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis;
    public float speed = 1.0f;
    public bool bobAnim = true;
    float counter;
    public float bobAmount = 0.2f;
    public float bobSpeed = 1.0f;
    Vector3 initPos;
    public AnimationCurve curve;

    private void Start() {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed * 360f * Time.deltaTime);

        if(bobAnim){
            transform.position = initPos + Vector3.up * bobAmount * curve.Evaluate(counter);

            counter += Time.deltaTime * bobSpeed;
            if(counter >= 1.0f){
                counter = 0.0f;
            }
        }
    }
}
