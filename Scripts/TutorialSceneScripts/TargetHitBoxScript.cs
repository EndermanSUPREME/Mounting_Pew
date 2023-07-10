using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitBoxScript : MonoBehaviour
{
    public int DamagePoints;
    [SerializeField] TargetHealthScript t_health;

    void Start()
    {
        if (GetComponent<TargetHealthScript>() == null) // head hitbox
        {
            if (transform.parent.GetComponent<TargetHealthScript>() != null)
            {
                // Debug.Log(transform.parent.name + ": Head");
                t_health = transform.parent.GetComponent<TargetHealthScript>();
            }
        } else // for main body hitbox
            {
                // Debug.Log(transform.name + ": Body");
                t_health = GetComponent<TargetHealthScript>();
            }
    }

    public void SendDamage()
    {
        t_health.TakeDamage(DamagePoints);
    }
}//EndScript