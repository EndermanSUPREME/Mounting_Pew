using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    [SerializeField] float speedConst, fieldRange, bulletSpeed;
    public Transform player, playerHead, middleSpine, upperSpine, Head, bulletSpawn;
    public AreaData NavArea;
    bool detectedPlayer = false, idle = false, isDead = false, isShooting = false, seekCover = false;
    [SerializeField] LayerMask playerMask, groundLayer;
    public GameObject[] weaponsAvaiable, headgear;
    public float[] rateOfFires;
    int gunSelect = 0;
    Vector3 dir2Player, coverPoint;
    [SerializeField] GameObject bulletPref;
    CoverDataScript coverGridData;

    [SerializeField] AudioSource[] gunSound;

    /*
        Enemy will initially wonder the area
        Enemy may detect the player
        Enemy will go after the player
        Enemy will rotate around to shoot at the player and adjust rotation when needed
        Enemy will find cover to attack from
    */

    void Awake()
    {
        if (NavArea == null)
        {
            NavArea = Object.FindObjectOfType<AreaData>();
        }

        player = GameObject.Find("playerBody").transform;
        playerHead = GameObject.Find("PlayerCamera").transform;

        gunSelect = Random.Range(0, weaponsAvaiable.Length - 1);

        coverGridData = GetComponent<CoverDataScript>();

        for (int i = 0; i < weaponsAvaiable.Length; i++)
        {
            if (i == gunSelect)
            {
                weaponsAvaiable[i].SetActive(true);
            } else
                {
                    weaponsAvaiable[i].SetActive(false);
                }
        }

        int headGearID = Random.Range(0, headgear.Length);

        for (int i = 0; i < headgear.Length; i++)
        {
            if (i == headGearID)
            {
                headgear[i].SetActive(true);
            } else
                {
                    headgear[i].SetActive(false);
                }
        }
    }

    public void BreakNavComps()
    {
        isDead = true;

        Destroy(coverGridData);
        Destroy(agent);
        Destroy(anim);

        Destroy(this);
    }

    public LayerMask GetEnvMask()
    {
        return groundLayer;
    }
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        switch (gunSelect)
        {
            case 0:
                anim.Play("pistol");
            break;
            case 1:
                anim.Play("shotgun");
            break;
            case 2:
                anim.Play("smg");
            break;
            case 3:
                anim.Play("bigGun");
            break;

            default:
            break;
        }

        agent.SetDestination(GetRandomDestination());
    }

    Vector3 GetRandomDestination()
    {
        float boundX = NavArea.GetAreaWidth()/2;
        float boundZ = NavArea.GetAreaLength()/2;

        Vector3 pointOnMap = new Vector3(Random.Range(-boundX, boundX), transform.position.y, Random.Range(-boundZ, boundZ));

        RaycastHit pointRay;
        if (Physics.Raycast(pointOnMap, Vector3.down, out pointRay, groundLayer))
        {
            return new Vector3(pointRay.point.x, agent.transform.position.y, pointRay.point.z);
        } else
            {
                return GetRandomDestination();
            }
    }

    IEnumerator IdlePeriod(int i)
    {
        // // Debug.Log("[+] Getting New Destination in " + i + " Seconds");
        idle = true;

        yield return new WaitForSeconds(i);

        if (!isDead)
        {
            agent.SetDestination(GetRandomDestination());
        }

        idle = false;
    }

    void Update()
    {
        if (!isDead)
        {
            Movement();
            FieldOfView();
        } else
            {
                StopAllCoroutines();
            }
    }

    public void AlertedByBullet()
    {
        if (!detectedPlayer)
        {
            // GetToCover();
            detectedPlayer = true;

            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, fieldRange * 2.5f);
        
            foreach (Collider c in nearbyObjects)
            {
                if (c.transform.GetComponent<EnemyLimb>() != null)
                {
                    c.transform.GetComponent<EnemyLimb>().AlarmSense();
                }
            }
        }
    }

    public void NearbyAlert()
    {
        Debug.Log("Alarm");
        detectedPlayer = true;
    }

    public Transform GetPlayerHead()
    {
        return playerHead;
    }

    public LayerMask GetPlayerLayer()
    {
        return playerMask;
    }

    void FieldOfView() /* ================== ATTACKING ======================= */
    {
        dir2Player = (playerHead.position - Head.position).normalized;

        // Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        // Debug.DrawRay(transform.position, dir2Player * 2, Color.cyan);

        if (!detectedPlayer)
        {
            if (Vector3.Angle(transform.forward, dir2Player) <= 45)
            {
                if (Physics.Raycast(Head.position, dir2Player, fieldRange, playerMask))
                {
                    // DecideForCover();
                    detectedPlayer = true;
                }
            }
        } else
            {
                if (Physics.Raycast(Head.position, dir2Player, fieldRange, playerMask))
                {
                    // Debug.Log("[+] Player Is Seen");

                    // Shoot at the player when the AI can see them
                    if (!isShooting)
                    {
                        isShooting = true;
                        StartCoroutine(Shoot());
                        
                        switch (gunSelect)
                        {
                            case 0:
                                anim.Play("attackPistol");
                            break;
                            case 1:
                                anim.Play("attackShotgun");
                            break;
                            case 2:
                                anim.Play("attackSMG");
                            break;
                            case 3:
                                anim.Play("attack_gripRifle");
                            break;

                            default:
                            break;
                        }
                    }
                } else
                    {
                        // Debug.Log("[-] Player Not Seen");

                        isShooting = false;
                    }
            }
    }

    IEnumerator Shoot()
    {
        if (isShooting)
        {
            GameObject nBullet = Instantiate(bulletPref, bulletSpawn.transform.position, bulletPref.transform.rotation);
            Vector3 dir2Player = (nBullet.transform.position - playerHead.position).normalized;

            gunSound[gunSelect].Play();
    
            nBullet.transform.LookAt(playerHead.position);
            nBullet.transform.GetComponent<Rigidbody>().velocity = nBullet.transform.forward * bulletSpeed;
    
            yield return new WaitForSeconds(rateOfFires[gunSelect]);
    
            StartCoroutine(Shoot());
        } else
            {
                yield return null;
            }
    }

    void Movement()
    {
        float dist2Dest = Vector3.Distance(agent.transform.position, agent.destination);

        if (!detectedPlayer)
        {
            if (dist2Dest < 0.1f && !idle)
            {
                StartCoroutine(IdlePeriod(Random.Range(3, 6)));
            }
        } else
            {
                if (!seekCover)
                {
                    agent.SetDestination(player.position);
                }
            }

        UpdateAnimation();
        AI_Speed();
    }

    void AI_Speed()
    {
        if (!detectedPlayer)
        {
            agent.speed = speedConst;
        } else
            {
                if (!isShooting)
                {
                    // Debug.Log("[*] Advance Target");

                    if (Vector3.Distance(agent.transform.position, agent.destination) > 4)
                    {
                        // run
                        if (agent.speed < speedConst * 1.75f)
                        {
                            agent.speed += 0.15f;
                        } else
                            {
                                agent.speed = speedConst * 1.75f;
                            }
                    } else
                        {
                            // walk
                            if (agent.speed > speedConst)
                            {
                                agent.speed -= 0.15f;
                            } else
                                {
                                    agent.speed = speedConst;
                                }
                        }
                } else if (isShooting)
                    {
                        if (Physics.Raycast(Head.position, dir2Player, fieldRange, playerMask))
                        {
                            // Debug.Log("[*] Stop And Shooting");

                            agent.speed = 0;
                        } else
                            {
                                agent.speed = speedConst;
                            }
                    }
            }
    }

    void LateUpdate()
    {
        if (!isDead && detectedPlayer)
        {
            if (Physics.Raycast(Head.position, dir2Player, fieldRange + 5, playerMask))
            {
                RotateBodyForAiming();
    
                if (Vector3.Angle(transform.forward, dir2Player) > 45)
                {
                    transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                }
            }
        }
    }

    void RotateBodyForAiming()
    {
        // rotate the AI torso and lift the arm and neck/head to aim at the player
        // if the rotation is too great then the ai needs to make a new choice on whether to shoot again or attack up-close

        middleSpine.LookAt(playerHead.position);
        // upperSpine.localRotation = new Quaternion(-4.879f, -25f, 0, 0);
    }

    void UpdateAnimation()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        anim.SetFloat("forwardSpeed", speed);
        anim.SetBool("shooting", isShooting);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fieldRange * 2.5f);
    }

}//EndScript