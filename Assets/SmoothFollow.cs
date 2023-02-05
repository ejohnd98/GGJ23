using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;

    private void LateUpdate() {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.1f * speed);
    }
}
