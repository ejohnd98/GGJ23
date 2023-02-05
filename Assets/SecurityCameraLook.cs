using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraLook : MonoBehaviour
{
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            transform.LookAt(target);
        }
    }
}
