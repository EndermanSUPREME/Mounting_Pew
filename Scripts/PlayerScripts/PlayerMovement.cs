using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    // Rigidbody PlayerRb;
    [SerializeField] Transform pointObject, playerCamera, playerFoot;
    [SerializeField] float speedConst, jumpHeight, maxClimbHeight, crouchSpeed, floorCheckRadius;
    float currentSpeed, rate = 0.15f, climbTime, particleSpeed;
    bool isGrounded, calcMount = false, isClimbing = false, standing = true, isSprinting = false, isSliding = false, replayWalkSound = true;
    Vector3 velocity, nPos, controllerCenter, camPos, slideDir;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] ParticleSystem speedParticles;
    [SerializeField] AudioSource footStepOne, footStepTwo;
    [SerializeField] AudioClip[] sandSteps, grassSteps, normalSteps;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        // PlayerRb = GetComponent<Rigidbody>();

        currentSpeed = speedConst;

        controllerCenter = controller.center;
        camPos = playerCamera.localPosition;

        crouchSpeed = 3;
        maxClimbHeight = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
    }

    void BasicMovement()
    {
        var main = speedParticles.main;

        if (isClimbing)
        {
            ClimbingMovement();
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (Input.GetKeyDown(KeyCode.LeftShift) && standing) // sprint switch
            {   
                if (isSprinting)
                    isSprinting = false; // walk
                else
                    speedParticles.Play();
                    isSprinting = true; // run
            }

            if (isSprinting && standing)
            {
                main.startSpeed = particleSpeed;
                particleSpeed = Mathf.Lerp(0, 1, 75 * Time.deltaTime);

                Debug.Log("Running!");

                if (z < 0.25f) isSprinting = false;

                if (standing)
                {
                    // Slide Trigger
                    if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
                    {
                        slideDir = transform.forward;

                        StartCoroutine(Slide());
                        isSprinting = false;
                    }

                    if (currentSpeed < speedConst * 2.15f)
                    {
                        currentSpeed += rate;
                    }
                    else
                    {
                        currentSpeed = speedConst * 2.15f;
                    }
                }
            } else // walking
                {
                    main.startSpeed = particleSpeed;
                    particleSpeed = Mathf.Lerp(1, 0, 75 * Time.deltaTime);

                    if (particleSpeed < 0.1f)
                    {
                        speedParticles.Stop();
                    }

                    if (isSliding)
                    {
                        Debug.DrawRay(transform.position, slideDir, Color.red);
                        controller.Move(slideDir * (speedConst * 2) * Time.deltaTime);
                        
                        if (z <= 0)
                        {
                            StopCoroutine(Slide());
                            isSliding = false;
                        }
                    }

                    if (currentSpeed > speedConst)
                    {
                        currentSpeed -= rate * 1.75f;
                    }
                    else
                    {
                        currentSpeed = speedConst;
                    }
                }

            if (!isSliding)
            {
                controller.Move(move * currentSpeed * Time.deltaTime);
            }

            GetFloorType();

            if ((Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0) && isGrounded)
            {
                if (currentSpeed <= speedConst)
                {
                    if (replayWalkSound)
                    {
                        StartCoroutine(footSteps(0.5f)); // walk
                    }
                } else
                    {
                        if (replayWalkSound)
                        {
                            StartCoroutine(footSteps(0.25f)); // run
                        }
                    }
            } else
                {
                    StopCoroutine(footSteps(0));
                }

            JumpMovement();
            Crouch();
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

    void GetFloorType()
    {
        // Debug.Log("[*] Finding Floor Type");

        Collider[] nearbyColliders = Physics.OverlapSphere(playerFoot.position, floorCheckRadius, groundLayer);

        if (nearbyColliders.Length > 0)
        {
            Collider floorCollider = nearbyColliders[0];

            Debug.Log("[+] Ray Hit" + " : " + floorCollider.transform.tag);

            if (floorCollider.transform.tag.Equals("grass"))
            {
                footStepOne.clip = grassSteps[0];
                footStepTwo.clip = grassSteps[1];
            }
            else if (floorCollider.transform.tag.Equals("sand"))
            {
                footStepOne.clip = sandSteps[0];
                footStepTwo.clip = sandSteps[1];
            }
            else if (floorCollider.transform.tag.Equals("normal"))
            {
                footStepOne.clip = normalSteps[0];
                footStepTwo.clip = normalSteps[1];
            }
        } else
            {
                Debug.Log("[-] No Collider Hit");
            }
    }

    void JumpMovement()
    {
        isGrounded = Physics.CheckSphere(playerFoot.position, 0.4f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pointObject != null)
                {
                    pointObject.localScale = Vector3.zero;
                }

                velocity.y = Mathf.Sqrt(jumpHeight * -2 * (-9.81f));
            }

            calcMount = false;
        }

        velocity.y += (-9.81f) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Crouch()
    {
        controller.center = controllerCenter;
        playerCamera.localPosition = camPos;

        controllerCenter.y = (0.5f * controller.height) - 1;
        camPos.y = (0.5f * controller.height) - 0.5f;

        if (standing)
        {
            controller.height = Mathf.Lerp(controller.height, 2, crouchSpeed * Time.deltaTime);
        } else
            {
                controller.height = Mathf.Lerp(controller.height, 1, crouchSpeed * Time.deltaTime);
            }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (standing)
                standing = false; // crouching
            else
                standing = true; // standing
        } else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!standing)
                {
                    standing = true;
                    isSprinting = true;
                    speedParticles.Play();
                }
            }
    }

    IEnumerator Slide()
    {
        isClimbing = false;

        isSliding = true;
        yield return new WaitForSeconds(1.15f);
        isSliding = false;
    }

    void TryClimbLedge(Vector3 collisionPoint)
    {
        Vector3 dir2Point = (collisionPoint - transform.position).normalized;

        float angle = Vector3.Angle(dir2Point, transform.forward);

        // Debug.Log("[*] Angle : " + angle);

        if (angle <= 45) // continue calculating
        {
            if (!calcMount)
            {
                // Debug.Log("[*] Finding Edge");
                Vector3 hitWallPoint;
                Vector3 playerForward;

                // Find the closest edge
                hitWallPoint = collisionPoint;
                playerForward = transform.forward;

                CalculateTheMount(hitWallPoint, playerForward);

                calcMount = true;
            }
        }
    }

    void CalculateTheMount(Vector3 hitWallPoint, Vector3 playerForward)
    {
        Vector3 climbPos = Vector3.zero;
        float initialHeight = hitWallPoint.y;

        // check for head obstructions after calculating mantle position
        // Debug.Log("[*] Finding New Mount Point");

        bool mntFound = false;
        while (!mntFound)
        {
            if (Physics.CheckSphere(hitWallPoint, 0.1f, groundLayer))
            {
                // increment hit-position by a unit until it stops hitting a collider
                if (pointObject != null)
                {
                    pointObject.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    pointObject.position = hitWallPoint;
                }

                hitWallPoint.y += 0.05f;
            } else
            {
                if ((Mathf.Abs(hitWallPoint.y - initialHeight) <= maxClimbHeight) && hitWallPoint.y > initialHeight && transform.position.y < hitWallPoint.y)
                {
                    // mount possition found
                    mntFound = true;
                    climbPos = hitWallPoint;
                } else
                    {
                        break;
                    }
            }
        }

        if (mntFound) // move the player to the new location
        {
            // Debug.Log("[+] Mount Point Found!");

            Vector3 nPos_base = climbPos + playerForward;
            nPos = nPos_base;

            RaycastHit heightCheck;
            if (Physics.Raycast(nPos_base, -Vector3.up * 2, out heightCheck, groundLayer))
            {
                nPos.y = heightCheck.point.y + 1;
            }

            if (pointObject != null)
            {
                pointObject.position = nPos;
                pointObject.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            climbTime = 0.1f;
            isClimbing = true;
        }
        else
        {
            // Debug.Log("[-] Mount Point Not Found!");
            if (pointObject != null)
            {
                pointObject.localScale = Vector3.zero;
                pointObject.position = Vector3.zero;
            }
        }
    }

    void ClimbingMovement()
    {
        // Debug.Log("[*] Climbing");
        velocity.y = 0;

        transform.position = Vector3.Lerp(transform.position, nPos, climbTime);

        climbTime += 2 * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, nPos) < 0.11f)
        {
            climbTime = 0;

            isClimbing = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isGrounded && !isSliding)
        {
            TryClimbLedge(hit.point);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(playerFoot.position, 0.4f);
    }
}//EndScript