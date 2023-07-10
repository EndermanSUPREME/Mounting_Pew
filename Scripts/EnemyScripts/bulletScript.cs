using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] GameObject impactParticle, impactSoundObject;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("[*] " + collision.gameObject.layer + ":" + TargetLayer.value);

        if ((TargetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Damage Target
            // Debug.Log("[+] Target Hit!");
            if (collision.transform.GetComponent<PlayerHealth>() != null)
            {
                collision.transform.GetComponent<PlayerHealth>().TakeDamage();
            }

            if (collision.transform.GetComponent<TargetHitBoxScript>() != null)
            {
                collision.transform.GetComponent<TargetHitBoxScript>().SendDamage();

                GameObject impactParticles = Instantiate(impactParticle, collision.contacts[0].point, impactParticle.transform.rotation);
                Destroy(impactParticles, 1);
            }

            if (collision.transform.GetComponent<EnemyLimb>() != null)
            {
                collision.transform.GetComponent<EnemyLimb>().TakeDamage();

                Collider[] nearbyLimbs = Physics.OverlapSphere(collision.contacts[0].point, 0.2f);
                foreach (Collider c in nearbyLimbs)
                {
                    if (c.transform.GetComponent<Rigidbody>() != null)
                    {
                        c.transform.GetComponent<Rigidbody>().AddExplosionForce(10, collision.contacts[0].point, 0.25f, 4);
                    }
                }
            }
        } else
            {
                GameObject impactParticles = Instantiate(impactParticle, collision.contacts[0].point, impactParticle.transform.rotation);
                Destroy(impactParticles, 1);
            }

        GameObject nSoundObj = Instantiate(impactSoundObject, collision.contacts[0].point, impactSoundObject.transform.rotation);
        Destroy(nSoundObj, 2);

        Destroy(gameObject);
    }
}//EndScript