using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealthScript : MonoBehaviour
{
    public int totalHealth;
    Animator animator;
    public bool practTarget = false;
    bool tarDown = false;
    SpeedRunAreaScript tutorialScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        tutorialScript = Object.FindObjectOfType<SpeedRunAreaScript>();

        if (practTarget)
        {
            TargetPropUp();
        }
    }

    public void TargetPropUp() // targets will rotate up for the player to shoot
    {
        animator.Play("targetPropUp");
        totalHealth = 5;
        tarDown = false;
    }

    public void TakeDamage(int amount)
    {
        if (!tarDown)
        {
            totalHealth -= amount;
    
            if (totalHealth <= 0)
            {
                TargetFallDown();
                totalHealth = 0;
                tarDown = true;
            }
        }
    }

    void TargetFallDown() // fall down when player deals sufficient damage
    {
        animator.Play("targetKnockedDown");

        if (practTarget)
        {
            Invoke("TargetPropUp", 1.15f);
        } else
            {
                tutorialScript.TargetBeenShotDown();
            }
    }

}//EndScript