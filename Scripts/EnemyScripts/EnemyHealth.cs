using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int totalHealth;
    [SerializeField] Rigidbody[] limbRbs;
    EnemyScript AI_Script;
    bool isDead = false;

    void Awake()
    {
        AI_Script = GetComponent<EnemyScript>();

        foreach (Rigidbody rb in limbRbs)
        {
            rb.isKinematic = true;
            rb.transform.GetComponent<Collider>().enabled = true;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isDead)
        {
            totalHealth -= amount;

            if (totalHealth <= 0)
            {
                Death();
            }

            transform.GetComponent<EnemyScript>().AlertedByBullet();
        }
    }

    public void AlarmedByOther()
    {
        if (transform.GetComponent<EnemyScript>() != null)
        {
            if (transform.GetComponent<EnemyScript>().enabled)
            {
                transform.GetComponent<EnemyScript>().NearbyAlert();
            }
        }
    }

    void Death()
    {
        isDead = true;

        AI_Script.BreakNavComps();

        foreach (Rigidbody rb in limbRbs)
        {
            rb.isKinematic = false;
            rb.transform.GetComponent<Collider>().enabled = true;
        }
    }
}//EndScript