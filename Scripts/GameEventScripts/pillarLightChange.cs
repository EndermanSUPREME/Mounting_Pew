using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarLightChange : MonoBehaviour
{
    float timeLeft;
    Color targetColor;
    [SerializeField] Renderer pillarGlowRend;
    [SerializeField] Light pillarLight;

    void Update()
    {
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            pillarGlowRend.material.color = targetColor;
            pillarGlowRend.material.SetColor("_EmissionColor", targetColor);
            pillarLight.color = targetColor;

            // start a new transition
            targetColor = new Color(Random.value, Random.value, Random.value);
            timeLeft = 1.0f;
        } else
            {
                // transition in progress
                // calculate interpolated color
                pillarGlowRend.material.color = Color.Lerp(pillarGlowRend.material.color, targetColor, Time.deltaTime / timeLeft);
                pillarLight.color = Color.Lerp(pillarGlowRend.material.color, targetColor, Time.deltaTime / timeLeft);
                pillarGlowRend.material.SetColor("_EmissionColor", pillarLight.color);

                // update the timer
                timeLeft -= Time.deltaTime;
            }
    }
}