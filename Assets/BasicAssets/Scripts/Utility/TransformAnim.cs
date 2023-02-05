using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformAnim : MonoBehaviour
{
    public enum AnimationType {Transform, Additive, Force};

    public AnimationType animType = AnimationType.Force;
    public bool setPos = false, setRot = false, setScale = false;
    public float duration = 1.0f;
    public AnimationCurve animCurve = AnimationCurve.Constant(0.0f, 1.0f, 1.0f);

    public Vector3 pos, rot, scale;
    public Transform animTransform;

    public string forwardSound, reverseSound;
    private bool reverse = true;

    private bool isAnimating = false;
    private float counter = 0.0f;
    private Vector3 posA, rotA, scaleA;
    private Vector3 posB, rotB, scaleB;    

    public bool startOnEnable = false;
    public UnityEvent onFinish;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateStartPos();
    }

    private void OnEnable() {
        if(startOnEnable){
            StartAnim();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimating){
            float adjustedTime = animCurve.Evaluate(counter);
            if (setPos) transform.position = Vector3.Lerp(posA, posB, adjustedTime);
            if (setRot) transform.localEulerAngles = Vector3.Lerp(rotA, rotB, adjustedTime);
            if (setScale) transform.localScale = Vector3.Lerp(scaleA, scaleB, adjustedTime);

            if(reverse){
                if(counter <= 0.0f){
                    isAnimating = false;
                    onFinish?.Invoke();
                }
                counter = Mathf.Clamp01(counter - Time.deltaTime / duration);
            }else{
                if(counter >= 1.0f){
                    isAnimating = false;
                    onFinish?.Invoke();
                }
                counter = Mathf.Clamp01(counter + Time.deltaTime / duration);
            }
        }
    }
    
    public void UpdateStartPos(){
        posA = transform.position;
        rotA = transform.localEulerAngles;
        scaleA = transform.localScale;

        if(animType == AnimationType.Transform){
            posB = animTransform.position;
            rotB = animTransform.localEulerAngles;
            scaleB = animTransform.localScale;
        }else if(animType == AnimationType.Additive){
            posB = posA + pos;
            rotB = rotA + rot;
            scaleB = new Vector3(scaleA.x * scale.x, scaleA.y * scale.y, scaleA.z * scale.z);
        }else if(animType == AnimationType.Force){
            posB = pos;
            rotB = rot;
            scaleB = scale;
        }
    }

    public void StartAnim(bool reverse = false){

        if(!isAnimating){
            counter = reverse? 1 : 0;

            //SoundSystem.instance.PlaySound(reverse? reverseSound: forwardSound, transform);
        }

        isAnimating = true;
        this.reverse = reverse;
    }

    public void ToggleAnim(){
        StartAnim(!reverse);
    }
}
