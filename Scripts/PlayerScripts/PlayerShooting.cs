using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform PlayerCamera, weaponPivot, pickUpWeaponIcon, bulletSpawn;
    [SerializeField] GameObject bulletPref;
    [SerializeField] GameObject[] equipped_Weapons, avaliableWeapons, weaponPickUpBase;
    [SerializeField] Text AmmoDisplay;
    [SerializeField] LayerMask weaponLayer, weaponPickUpLayer;
    int[] weaponTotalAmmo = {35, 75, 12}, weaponMags = {7, 25, 6}, weaponMagsInfo; // ammo display will be weaponMags/weaponTotalAmmo ===> weaponMagInfo is not being displayed it is used for logic calcs!!!
    int equippedIndex = 0, bulletRange;
    float rateOfFire, timer;
    Animator weaponAnim;
    bool swappingWeapons = false, pickingUpWeapon = false, reloading = false, firing = false;
    AudioSource equipGunSound;
    [SerializeField] AudioSource swapSound, m1911a1, m9Barretta, revolver, uzi, AR, Stubby, semiShotty, bar, huntingRifle, unscopedRifle, dmr;

    void Start()
    {
        weaponMagsInfo = new int[3];

        Set_WeaponDetails();
        equippedIndex = 0;

        weaponAnim = equipped_Weapons[equippedIndex].GetComponent<Animator>();
        // weaponAnim.Play("bringUp");
    }

    public void CollectMoreAmmo(int a, int b, int c)
    {
        weaponTotalAmmo[0] += a;
        weaponTotalAmmo[1] += b;
        weaponTotalAmmo[2] += c;

        for (int i = 0; i < 3; i++)
        {
            if (weaponTotalAmmo[i] > 250)
            {
                weaponTotalAmmo[i] = 250;
            }
        }
    }

    void Set_WeaponDetails()
    {
        weaponAnim = equipped_Weapons[equippedIndex].GetComponent<Animator>();

        switch (equipped_Weapons[equippedIndex].transform.name)
        {
            case "m1911a1":
                weaponMagsInfo[0] = 7;
                rateOfFire = 0.133f / 2;

                Debug.Log(rateOfFire);

                if (weaponMags[equippedIndex] > weaponMagsInfo[0])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[0];
                    weaponMags[equippedIndex] = weaponMagsInfo[0];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = m1911a1;
                break;
            case "m9Barretta":
                weaponMagsInfo[0] = 15 / 2;
                rateOfFire = 0.167f;

                if (weaponMags[equippedIndex] > weaponMagsInfo[0])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[0];
                    weaponMags[equippedIndex] = weaponMagsInfo[0];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = m9Barretta;
                break;
            case "revolver":
                weaponMagsInfo[0] = 6;
                rateOfFire = 0.167f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[0])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[0];
                    weaponMags[equippedIndex] = weaponMagsInfo[0];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = revolver;
                break;
            case "uzi":
                weaponMagsInfo[0] = 20;
                rateOfFire = 0.125f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[0])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[0];
                    weaponMags[equippedIndex] = weaponMagsInfo[0];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = uzi;
                break;
            //=====================
            case "AR":
                weaponMagsInfo[1] = 25;
                rateOfFire = 0.133f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[1])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[1];
                    weaponMags[equippedIndex] = weaponMagsInfo[1];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = AR;
                break;
            case "Stubby":
                weaponMagsInfo[1] = 21;
                rateOfFire = 0.167f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[1])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[1];
                    weaponMags[equippedIndex] = weaponMagsInfo[1];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = Stubby;
                break;
            case "semiShotty":
                weaponMagsInfo[1] = 8;
                rateOfFire = 0.167f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[1])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[1];
                    weaponMags[equippedIndex] = weaponMagsInfo[1];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = semiShotty;
                break;
            case "bar":
                weaponMagsInfo[1] = 30;
                rateOfFire = 0.167f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[1])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[1];
                    weaponMags[equippedIndex] = weaponMagsInfo[1];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = bar;
                break;
            //=====================
            case "huntingRifle":
                weaponMagsInfo[2] = 6;
                rateOfFire = 0.75f;

                if (weaponMags[equippedIndex] > weaponMagsInfo[2])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[2];
                    weaponMags[equippedIndex] = weaponMagsInfo[2];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = huntingRifle;
                break;
            case "unscopedRifle":
                weaponMagsInfo[2] = 6;
                rateOfFire = 0.6f;

                if (weaponMags[equippedIndex] > weaponMagsInfo[2])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[2];
                    weaponMags[equippedIndex] = weaponMagsInfo[2];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = unscopedRifle;
                break;
            case "dmr":
                weaponMagsInfo[2] = 15;
                rateOfFire = 0.167f / 2;

                if (weaponMags[equippedIndex] > weaponMagsInfo[2])
                {
                    int extraBullets = weaponMags[equippedIndex] - weaponMagsInfo[2];
                    weaponMags[equippedIndex] = weaponMagsInfo[2];

                    weaponTotalAmmo[equippedIndex] += extraBullets;
                }

                equipGunSound = dmr;
                break;

            default:
                break;
        }

        weaponAnim.Play("bringUp");
    }

    void Update()
    {
        if (!pickingUpWeapon || !swappingWeapons)
        {
            WeaponMovement_Affordance();

            if (!reloading)
            {
                UseWeapon();
            }

            WeaponSwap();
            FindDroppedWeapon();
        }

        AmmoDisplay.text = weaponMags[equippedIndex].ToString() + "/" + weaponTotalAmmo[equippedIndex].ToString();
    }

    void WeaponSwap()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            weaponAnim.Play("swap");
            StartCoroutine(switchEquippedLogic(-1, weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / 10));
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) // backwards
            {
                weaponAnim.Play("swap");
                StartCoroutine(switchEquippedLogic(1, weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / 10));
            }

        if (weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("reload"))
        {
            if (weaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                if (reloading)
                {
                    reloading = false;
                }
            }
        }
    }

    IEnumerator switchEquippedLogic(int dir, float duration)
    {
        swappingWeapons = true;
        firing = false;

        if (reloading)
        {
            reloading = false;

        }

        // Debug.Log("[*] Swap started! : " + duration);

        yield return new WaitForSeconds(duration);

        // Debug.Log("[*] Swap completed!");

        swappingWeapons = false;

        if (dir > 0) // up
        {
            if (equippedIndex < 2)
            {
                equippedIndex++;
            } else
                {
                    equippedIndex = 0;
                }
        } else // down
            {
                if (equippedIndex > 0)
                {
                    equippedIndex--;
                } else
                    {
                        equippedIndex = 2;
                    }
            }

        Set_WeaponDetails();
        swapSound.Play();

        for (int i = 0; i < avaliableWeapons.Length; i++)
        {
            if (avaliableWeapons[i] == equipped_Weapons[equippedIndex])
                equipped_Weapons[equippedIndex].SetActive(true);
            else
                avaliableWeapons[i].SetActive(false);
        }
    }

    void WeaponMovement_Affordance()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        weaponPivot.localPosition = new Vector3(0.5f,-0.25f,0.75f) + new Vector3(x*0.05f, 0, -z*0.05f);
    }

    bool HasParameter(Animator animator, string paramName)
    {
        foreach(AnimatorControllerParameter param in animator.parameters)
        {
            if(param.name.Equals(paramName)) return true;
        }

        return false;
    }

    void UseWeapon()
    {
        string weaponName = equipped_Weapons[equippedIndex].transform.name;

        // Debug.Log(!reloading && weaponMags[equippedIndex] > 0);

        // list off weapon tags due to some weapons being full fire and others are semi fire
        if (!reloading && weaponMags[equippedIndex] > 0)
        {
            if (weaponName.Equals("m1911a1") || weaponName.Equals("m9Barretta") || weaponName.Equals("revolver") || weaponName.Equals("semiShotty") || weaponName.Equals("huntingRifle") || weaponName.Equals("unscopedRifle") || weaponName.Equals("dmr"))
            {
                if (Input.GetMouseButtonDown(0)) // single-shot
                {
                    if (!firing)
                    {
                        Debug.Log(rateOfFire);
                        StartCoroutine(RaycastPlayerBullet(rateOfFire));
                    }
                }
            } else
                {
                    if (Input.GetMouseButton(0)) // full-auto
                    {
                        weaponAnim.SetBool("mouseDown", true);
                        if (!firing)
                        {
                            StartCoroutine(RaycastPlayerBullet(rateOfFire));
                        }
                    } else
                        {
                            weaponAnim.SetBool("mouseDown", false);
                        }
                }
        } else
            {
                if (HasParameter(weaponAnim, "mouseDown"))
                {
                    weaponAnim.SetBool("mouseDown", false);
                }
            }

        if (Input.GetKeyDown(KeyCode.R) && weaponMags[equippedIndex] < weaponMagsInfo[equippedIndex]) // reloading
        {
            reloading = true;
            weaponAnim.Play("reload");
        }

        if (weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("reload"))
        {
            if (weaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                int missingRounds = weaponMagsInfo[equippedIndex] - weaponMags[equippedIndex];
    
                // Debug.Log(missingRounds);
    
                if (weaponTotalAmmo[equippedIndex] - missingRounds >= 0)
                {
                    weaponTotalAmmo[equippedIndex] -= missingRounds;
                    weaponMags[equippedIndex] += missingRounds;
                } else
                    {
                        weaponMags[equippedIndex] += weaponTotalAmmo[equippedIndex];
                        weaponTotalAmmo[equippedIndex] = 0;
                    }
    
                reloading = false;
            }
        }
    }

    IEnumerator RaycastPlayerBullet(float rOf)
    {
        Debug.Log(rateOfFire);

        firing = true;
        
        equipGunSound.Play();

        GameObject nBullet = Instantiate(bulletPref, bulletSpawn.position, bulletPref.transform.rotation);

        Vector3 bulletDirection = transform.position + transform.forward * 1000;
        nBullet.transform.LookAt(bulletDirection);

        nBullet.GetComponent<Rigidbody>().velocity = nBullet.transform.forward * 35;

        if (equipped_Weapons[equippedIndex].layer != 7) // full-autos
        {
            weaponAnim.Play("shoot", -1, 0);
        } else // semi-auto
            {
                weaponAnim.Play("shoot");
            }

        switch (equippedIndex)
        {
            case 0:
                weaponMags[0]--;
            break;
            case 1:
                weaponMags[1]--;
            break;
            case 2:
                weaponMags[2]--;
            break;

            default:
            break;
        }

        yield return new WaitForSeconds(rOf);
        firing = false;
    }

    void FindDroppedWeapon()
    {
        RaycastHit hit;

        if (!pickingUpWeapon)
        {
            if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, 2.15f, weaponPickUpLayer))
            {
                string[] equippedGunNames = {equipped_Weapons[0].transform.name, equipped_Weapons[1].transform.name, equipped_Weapons[2].transform.name};

                if (!equippedGunNames.Contains(hit.transform.name))
                {
                    pickUpWeaponIcon.gameObject.SetActive(true);
    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        timer = Time.time;
                    }
        
                    // show affordance
                    if (Input.GetKey(KeyCode.E) && Time.time - timer >= 0.5f)
                    {
                        // Swap Weapon
                        weaponAnim.Play("swap");
                        StartCoroutine(swapForNewWeapon(hit.transform.gameObject, weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / 10));
                        pickingUpWeapon = true;
                    }
                } else
                    {
                        pickUpWeaponIcon.gameObject.SetActive(false);
                    }
            } else
                {
                    pickUpWeaponIcon.gameObject.SetActive(false);
                }
        } else
            {
                pickUpWeaponIcon.gameObject.SetActive(false);
            }
    }

    IEnumerator swapForNewWeapon(GameObject newGun, float dur)
    {
        yield return new WaitForSeconds(dur);

        switch (newGun.transform.gameObject.layer)
        {
            case 10:
                searchForWeapon(newGun.transform.gameObject, 0, 10);
                break;
            case 11:
                searchForWeapon(newGun.transform.gameObject, 1, 11);
                break;
            case 12:
                searchForWeapon(newGun.transform.gameObject, 2, 12);
                break;

            default:
                break;
        }
    }

    void searchForWeapon(GameObject desiredWeapon, int weaponType, int assignedLayer)
    {
        GameObject oldWeapon = equipped_Weapons[weaponType];
        GameObject r_gun = null;
        Vector3 g_rot = desiredWeapon.transform.eulerAngles;
        Vector3 g_pos = desiredWeapon.transform.position;

        foreach (GameObject weapon in avaliableWeapons)
        {
            if (weapon.transform.name.Equals(desiredWeapon.transform.name))
            {
                equipped_Weapons[weaponType] = weapon;

                Destroy(desiredWeapon); // remove weapon from the scene

                foreach (GameObject baseWeapon in weaponPickUpBase)
                {
                    if (baseWeapon.transform.name.Equals(oldWeapon.transform.name))
                    {
                        r_gun = Instantiate(baseWeapon); // replace the removed weapon
                    }
                }

                r_gun.SetActive(true);
                
                if (r_gun.GetComponent<Animator>() != null)
                {
                    Destroy(r_gun.GetComponent<Animator>()); // remove the animator & update the position
                }

                r_gun.layer = assignedLayer;

                foreach (Transform child in r_gun.GetComponentInChildren<Transform>())
                {
                    foreach (Transform grand_child in child.GetComponentInChildren<Transform>())
                    {
                        grand_child.gameObject.layer = assignedLayer;

                        foreach (Transform great_grand_child in grand_child.GetComponentInChildren<Transform>())
                        {
                            great_grand_child.gameObject.layer = assignedLayer;
                        }
                    }

                    child.gameObject.layer = assignedLayer;
                }

                r_gun.transform.name = r_gun.transform.name.Substring(0, r_gun.transform.name.Length - 7); // remove -> (Clone) from Instantiated Object Name
                r_gun.transform.position = g_pos;
                r_gun.transform.eulerAngles = g_rot;
            }
        }

        for (int i = 0; i < avaliableWeapons.Length; i++)
        {
            if (avaliableWeapons[i] == equipped_Weapons[equippedIndex])
                equipped_Weapons[equippedIndex].SetActive(true);
            else
                avaliableWeapons[i].SetActive(false);
        }

        Set_WeaponDetails();

        pickingUpWeapon = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1000);
    }

}//EndScript