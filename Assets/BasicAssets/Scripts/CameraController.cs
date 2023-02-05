using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float followSpeed = 20f;
    public bool matchRotation = false;

    public Vector3 currentVelocity;

    public float smoothHorz = 0.7f, smoothVert = 0.4f;
    float yVelocity = 0.0f;
    float xVelocity = 0.0f;

    public bool onUpdate, onLateUpdate;

    void Update()
    {
        if(target && onUpdate){
            UpdateFollow();
        }
    }

    private void LateUpdate() {
        if(target && onLateUpdate){
            UpdateFollow();
        }
    }

    private void UpdateFollow(){
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, 1.0f/followSpeed);
        if(matchRotation){
                float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, smoothHorz);
                float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.eulerAngles.x, ref xVelocity, smoothVert);

                transform.rotation = Quaternion.Euler(xAngle, yAngle, 0);
            
        }
    }
}
