using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLimb : MonoBehaviour
{
    [SerializeField] EnemyHealth healthScript;
    [SerializeField] int HitDamage;

    public void TakeDamage()
    {
        healthScript.TakeDamage(HitDamage);
    }

    public void AlarmSense()
    {
        healthScript.AlarmedByOther();
    }
}//EndScript