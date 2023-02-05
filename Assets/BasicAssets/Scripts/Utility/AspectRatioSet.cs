using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioSet : MonoBehaviour
{

    AspectRatioFitter aspectRatioFitter;
    // Start is called before the first frame update
    void Start()
    {
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectRatio = Screen.width / Screen.height;
    }
}
