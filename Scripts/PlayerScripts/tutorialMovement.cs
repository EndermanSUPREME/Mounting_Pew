using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialMovement : MonoBehaviour
{
    public bool runMouseCode;
    bool replayWalkSound = true;

    void Update()
    {
        if (runMouseCode)
        {
            MouseLook();
        } else
            {
                KeyboardMovement();
            }
    }

//====================================================================================================
//====================================================================================================
//====================================================================================================

    public float mouseSensitivity = 200f;
    [SerializeField] Transform playerBody;
    float xRotation = 0f;

    [SerializeField] AudioSource footStepOne, footStepTwo;

    // Update is called once per frame
    void MouseLook()
    {
        Application.targetFrameRate = 65;

        ForceHideCursor();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void ForceHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

//====================================================================================================
//====================================================================================================
//====================================================================================================

    CharacterController controller;
    [SerializeField] float speedConst;
    Vector3 velocity;

    void KeyboardMovement()
    {
        controller = GetComponent<CharacterController>();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        PlayFootStepSounds(x, z);

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speedConst * Time.deltaTime);

        velocity.y = -2f;
        controller.Move(velocity * Time.deltaTime);
    }

    void PlayFootStepSounds(float x, float z)
    {
        if ((Mathf.Abs(x) > 0.25f || Mathf.Abs(z) > 0.25f))
        {
            if (replayWalkSound)
            {
                StartCoroutine(footSteps(0.65f)); // walk
            }
        }
        else
        {
            StopCoroutine(footSteps(0));
        }
    }

    IEnumerator footSteps(float t)
    {
        replayWalkSound = false;

        footStepOne.Play();
        yield return new WaitForSeconds(t);
        footStepTwo.Play();
        yield return new WaitForSeconds(t);
        replayWalkSound = true;
    }

}//EndScript