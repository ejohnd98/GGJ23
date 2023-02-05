using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 2.5f;
    public float turnSpeed = 80.0f;
    public Transform cameraPivot;
    public Transform sceneCam, weaponCam, mouseLookPivot;

    public AnimationCurve headBobCurve;
    public float headBobIntensity = 0.05f;
    public float headBobWeaponIntensity = 0.04f;
    public float headBobSpeed = 2.0f;
    public float viewAngle = 70f;
    float headbobCounter = 0.0f;
    float initialCameraOffset;
    float initialWeaponOffset;
    bool headBobActive = false;
    float stepSoundCooldown = 0.0f;

    public float mouseLookX = 3f, mouseLookY = 2.5f;
    public float mouseSensitivity = 1.0f;

    public LayerMask groundLayer;
    public float playerHeight = 0.7f;
    public float movementSnappiness = 5.0f;
    public float fallSpeed = 40.0f;

    public bool freezePosition = false, freezeRotation = false;

    bool falling = false;

    public Vector3 velTarget;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialCameraOffset = sceneCam.localPosition.y;
        initialWeaponOffset = weaponCam.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if(stepSoundCooldown > 0.0f){
            stepSoundCooldown -= Time.deltaTime;
        }
    }

    public void ChangeMouseSensitivity(float amount){
        mouseSensitivity = Mathf.Clamp(mouseSensitivity+amount, 0.1f, 3.0f);
        UISystem.ShowMousePrompt(mouseSensitivity);
    }

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
    }

    void FixedUpdate() {
        HandlePhysics();
        HandleMovement();
        if(!freezePosition){
            HandleHeadBob();
        }
        
    }

    void HandleHeadBob(){
        if(headBobActive && !falling){
            headbobCounter += Time.deltaTime * headBobSpeed;
            if(headbobCounter > 1.0f){
                headbobCounter = 0.0f;
                PlayStepSound();
            }
        }else{
            if(headbobCounter > 0.5f && headbobCounter < 1.0f){
                headbobCounter += Time.deltaTime * headBobSpeed;

                if(headbobCounter > 1.0f){
                    headbobCounter = 0.0f;
                }
            }else if(headbobCounter > 0.0f){
                headbobCounter -= Time.deltaTime * headBobSpeed;
                if(headbobCounter < 0.0f){
                    headbobCounter = 0.0f;
                }
            }
        }

        Vector3 sceneCamPos = sceneCam.localPosition;
        sceneCamPos.y = initialCameraOffset - (headBobIntensity * headBobCurve.Evaluate(headbobCounter));
        sceneCam.localPosition = sceneCamPos;

        Vector3 weaponCamPos = weaponCam.localPosition;
        float weaponBobCounter = headbobCounter + 0.1f;
        if(weaponBobCounter > 1.0f){
            weaponBobCounter -= 1.0f;
        }
        weaponCamPos.y = initialWeaponOffset - (headBobWeaponIntensity * headBobCurve.Evaluate(weaponBobCounter));
        weaponCam.localPosition = weaponCamPos;
    }

    void HandleInput(){

        if(InputHandler.GetGenericDown(KeyCode.Plus) || InputHandler.GetGenericDown(KeyCode.Equals)){
            ChangeMouseSensitivity(0.1f);
        }else if(InputHandler.GetGenericDown(KeyCode.Minus) || InputHandler.GetGenericDown(KeyCode.Underscore)){
            ChangeMouseSensitivity(-0.1f);
        }



        if(!freezePosition){
            Vector3 directionVector = (transform.right * InputHandler.GetHorizontalAxis()) + (transform.forward * InputHandler.GetVerticalAxis());

            if(directionVector.magnitude > 0.01f){
                if(!headBobActive && !falling){
                    PlayStepSound();
                }
                headBobActive = true;
            }else{
                headBobActive = false;
            }
            velTarget = directionVector.normalized * movementSpeed + (velTarget.y * Vector3.up);
        }else{
            velTarget = Vector3.zero;
        }

        float turnAmount = mouseSensitivity * Mathf.Clamp(Input.GetAxis ("Mouse X"), -5f, 5f);
        float lookAmount = mouseSensitivity * Mathf.Clamp(-Input.GetAxis ("Mouse Y"), -5f, 5f);

        float xScale = 1920f/Screen.width;
        float yScale = 1080f/Screen.width;

        if(!freezeRotation){
            transform.Rotate(Vector3.up * turnAmount * xScale * turnSpeed * mouseLookX * 0.016f);
            mouseLookPivot.RotateAround(mouseLookPivot.position, mouseLookPivot.right, lookAmount * yScale * turnSpeed * mouseLookY * 0.016f);
        
            Vector3 camRot =  mouseLookPivot.localEulerAngles;
            camRot.y = 0f;
            camRot.z = 0f;
            float xAngle = camRot.x;
            while(xAngle < 180.0f){
                xAngle += 360.0f;
            }
            while(xAngle > 180.0f){
                xAngle -= 360.0f;
            }
            xAngle = Mathf.Clamp(xAngle, -viewAngle, viewAngle);

            camRot.x = xAngle;
            mouseLookPivot.localEulerAngles = camRot;
        }
    }

    void HandleMovement(){
        Vector3 velCurrent = rb.velocity;
        Vector3 velDiff = velTarget - velCurrent;

        rb.AddForce(velDiff * Time.fixedDeltaTime * movementSnappiness, ForceMode.VelocityChange);
    }

    void HandlePhysics(){

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 10.0f, groundLayer)){
            
            if(hit.point.y + playerHeight < transform.position.y){
                falling = true;
            }else{
                transform.position = hit.point + Vector3.up*playerHeight;
                if(falling && velTarget.y < -2.0f){
                    PlayStepSound();
                }
                falling = false;
                velTarget.y = 0.0f;
            }
        }else{
            falling = true;
        }

        if(falling){
            velTarget.y -= fallSpeed*Time.fixedDeltaTime;
        }
    }

    void PlayStepSound(){
        if(stepSoundCooldown <= 0.0f){
            SoundSystem.PlaySoundStatic("step");
            stepSoundCooldown = 0.2f;
        }
    }
}
