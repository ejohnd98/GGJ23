using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    Image target;
    public Sprite sprite1, sprite2;
    bool setTo1 = true;
    public float interval = 1.0f;
    float counter = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Image>();
        target.sprite = sprite1;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter > interval){
            counter = 0;
            setTo1 = !setTo1;
            target.sprite = setTo1? sprite1 : sprite2;
        }
    }
}
