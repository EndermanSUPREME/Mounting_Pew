using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ammoPack : MonoBehaviour
{
    PlayerShooting playerAmmo;
    AudioSource pickUpSound;

    // Start is called before the first frame update
    void Start()
    {
        playerAmmo = Object.FindObjectOfType<PlayerShooting>();
        pickUpSound = GameObject.Find("pickUpSound").transform.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.name.Equals("playerBody"))
        {
            int a = Random.Range(3, 8);
            int b = Random.Range(5, 9);
            int c = Random.Range(2, 5);

            playerAmmo.CollectMoreAmmo(a, b, c);

            pickUpSound.Play();

            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                Destroy(gameObject);
            }
        }
    }
}//EndScript