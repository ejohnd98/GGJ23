using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{

    public Transform target;
    public bool holdY = true;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameController.instance.playerCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(target){
            Vector3 targetPos = target.position;
            if(holdY)
                targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }
}
