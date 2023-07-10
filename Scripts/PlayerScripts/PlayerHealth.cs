using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int totalHealth;
    int maxHealth;
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerMovement movementScript;
    [SerializeField] GameObject WeaponGroup, GameOverScreen, healthBar;
    [SerializeField] MenuScript mainUIScript;
    bool isDead = false, invincible = false;
    [SerializeField] AudioSource hurtOne, hurtTwo;

    void Awake()
    {
        maxHealth = totalHealth;
        GameOverScreen.SetActive(false);
    }

    public void TakeDamage()
    {
        if (!isDead && !invincible)
        {
            if (Random.Range(0, 100) % 2 == 0)
            {
                hurtOne.Play();
            } else
                {
                    hurtTwo.Play();
                }

            totalHealth--;

            float scaleX = (float)totalHealth/(float)maxHealth;

            healthBar.transform.localScale = new Vector3(scaleX, 1, 1);
    
            if (totalHealth <= 0)
            {
                isDead = true;
                Death();
            }
        }
    }

    public void GoingToNextLevel()
    {
        invincible = true;
    }

    public void DeathByWater()
    {
        totalHealth = -1000;

        TakeDamage();
    }

    void Death()
    {
        Destroy(controller);
        Destroy(movementScript);
        Destroy(GetComponent<PlayerCameraScript>());
        Destroy(GetComponent<PlayerShooting>());
        Destroy(WeaponGroup);

        GetComponent<Animator>().enabled = true;

        Invoke("TryAgainScreen", 1);
    }

    void TryAgainScreen()
    {
        mainUIScript.HasDied();
        GameOverScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}//EndScript