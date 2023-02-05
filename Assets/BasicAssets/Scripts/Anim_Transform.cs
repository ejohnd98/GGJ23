using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct AnimKey{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public AnimKey(Transform transform, bool local){
        pos = local? transform.localPosition : transform.position;
        rot = local? transform.localRotation : transform.rotation;
        scale = transform.localScale;
    }
}

public class Anim_Transform : MonoBehaviour
{
    //Public
    public Transform TransformA, TransformB;
    public bool localAnim = true;
    public float animLength;
    public EasingType easing = EasingType.LINEAR;
    public bool animColor = false;
    public bool animTransform = true;
    public Color endColor;

    public bool playOnStart = false;
    public bool startReversed = false;
    public bool autoReverseOnFinish = false;

    public UnityEvent OnFinished;
    public string startSound, startReversedSound, finishSound;

    //Debug
    public bool toggleSwitch = false;

    //Private
    AnimKey keyA, keyB;
    bool IsPlaying = false;
    bool IsReverse = true; //start true so that toggling will play forwards
    float counter = 0.0f;
    MeshRenderer meshRenderer;
    Color startColor;
    Material currentMat;

    //Public Functions
    public void PlayAnim(bool reverse = false){
        if(!IsPlaying){
            counter = reverse ? 1.0f: 0.0f;
        }
        
        // Only play if starting or if direction changes
        if(startSound.Length > 0 && (!IsPlaying || (IsReverse != reverse))){
            SoundSystem.PlaySoundStatic(reverse? startReversedSound : startSound, transform);
        }

        IsPlaying = true;
        IsReverse = reverse;
    }

    public void ToggleAnim(){
        PlayAnim(!IsReverse);
    }

    //Private Functions
    private void Awake()
    {
        if(animTransform){
            keyA = new AnimKey(TransformA, localAnim);
            keyB = new AnimKey(TransformB, localAnim);
        }

        if(animColor){
            meshRenderer = GetComponent<MeshRenderer>();
            currentMat = meshRenderer.material;
            startColor = currentMat.color;
        }
    }

    private void Start() {
        if(playOnStart){
            PlayAnim(startReversed);
        }
    }

    void Update()
    {
        if(toggleSwitch){
            toggleSwitch = false;
            ToggleAnim();
        }
        if(IsPlaying){
            counter += (Time.deltaTime / animLength * (IsReverse? -1.0f : 1.0f));
            
            if((!IsReverse && counter >= 1.0f) || (IsReverse && counter <= 0.0f)){
                IsPlaying = false;

                if(finishSound.Length > 0){
                    SoundSystem.PlaySoundStatic(finishSound, transform);
                }

                if(autoReverseOnFinish && !IsReverse){
                    PlayAnim(true);
                }else{
                    OnFinished.Invoke();
                }
            }
            if(animTransform){
                UpdateAnim(counter);
            }   
            if(animColor){
                UpdateColor(counter);
            }
        }
    }

    void UpdateAnim(float progress){
        float easedProgress = Easings.EaseIn(progress, easing);
        
        if(localAnim){
            transform.localPosition = Vector3.Lerp(keyA.pos, keyB.pos, easedProgress);
            transform.localRotation = Quaternion.Lerp(keyA.rot,keyB.rot,easedProgress);
            transform.localScale = Vector3.Lerp(keyA.scale,keyB.scale,easedProgress);
        }else{
            transform.position = Vector3.Lerp(keyA.pos,keyB.pos,easedProgress);
            transform.rotation = Quaternion.Lerp(keyA.rot,keyB.rot,easedProgress);
            transform.localScale = Vector3.Lerp(keyA.scale,keyB.scale,easedProgress); //can't set global scale 
        }
    }

    void UpdateColor(float progress){
        float easedProgress = Easings.EaseIn(progress, easing);

        currentMat = meshRenderer.material;
        Color currentColor = currentMat.color;
        
        currentColor.a = Mathf.Lerp(startColor.a, endColor.a, easedProgress);
        currentColor.r = Mathf.Lerp(startColor.r, endColor.r, easedProgress);
        currentColor.g = Mathf.Lerp(startColor.g, endColor.g, easedProgress);
        currentColor.b = Mathf.Lerp(startColor.b, endColor.b, easedProgress);

        currentMat.color = currentColor;
        meshRenderer.material = currentMat;
    }
}
